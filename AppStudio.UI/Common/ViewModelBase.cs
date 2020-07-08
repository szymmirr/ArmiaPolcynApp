using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using AppStudio.Data;
using AppStudio.Services;

namespace AppStudio
{
    public enum ViewTypes
    {
        List,
        Detail
    }

    abstract public class ViewModelBase : BindableBase
    {
        private Visibility _progressBarVisibility = Visibility.Visible;

        public Visibility ProgressBarVisibility
        {
            get { return _progressBarVisibility; }
            set { SetProperty(ref _progressBarVisibility, value); }
        }

        public bool IsAppBarVisible
        {
            get
            {
                return IsSpeakTextVisible |
                       IsPinToStartVisible |
                       IsGoToSourceVisible |
                       IsShareItemVisible |
                       IsRefreshVisible;
            }
        }

        virtual public ViewTypes ViewType { get; set; }

        virtual public bool IsSpeakTextVisible
        {
            get { return false; }
        }

        virtual public bool IsPinToStartVisible
        {
            get { return false; }
        }

        virtual public bool IsGoToSourceVisible
        {
            get { return false; }
        }

        virtual public bool IsShareItemVisible
        {
            get { return false; }
        }

        virtual public bool IsRefreshVisible
        {
            get { return false; }
        }

        public async void Refresh()
        {
            await RefreshItems();
        }

        abstract public Task LoadItems(bool isNetworkAvailable);
        abstract public void LoadItem(string id);

        abstract public Task RefreshItems();

        virtual public void SpeakText() { }
        virtual public void PinToStart() { }
        virtual public void GoToSource() { }
        virtual public void ShareItem() { }

        virtual public void ImageTap(string url)
        {
            if (!String.IsNullOrEmpty(url))
            {
                NavigationServices.NavigateToPage("ImageViewer", "url=" + HttpUtility.UrlEncode(url));
            }
        }
    }

    abstract public class ViewModelBase<T> : ViewModelBase where T : BindableSchemaBase
    {
        protected ObservableCollection<T> _items = null;

        protected T _navigationItem = null;
        protected T _selectedItem = null;

        protected IDataSource<T> _dataSource;

        public ObservableCollection<T> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public T NavigationItem
        {
            get { return _navigationItem; }
            set
            {
                _navigationItem = value;
                if (_navigationItem != null)
                {
                    SelectedItem = _navigationItem;
                    NavigationServices.CurrentViewModel = this;
                    NavigateToSelectedItem();
                    SetProperty(ref _navigationItem, null);
                }
            }
        }

        public T SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public bool IsItemSelected
        {
            get { return SelectedItem != null; }
        }

        public IDataSource<T> DataSource
        {
            get { return _dataSource ?? (_dataSource = CreateDataSource()); }
        }

        virtual public bool IsStaticData
        {
            get { return false; }
        }

        override public async Task LoadItems(bool isNetworkAvailable)
        {
            ProgressBarVisibility = Visibility.Visible;

            if (!IsStaticData)
            {
                if (_kindOfDataLoaded == KindOfDataLoaded.Empty)
                {
                    // Read items from Cache
                    var records = AppCache.GetItems<T>(CacheKey);
                    if (records != null)
                    {
                        _kindOfDataLoaded = KindOfDataLoaded.FromCache;
                        Items = new ObservableCollection<T>(records);
                    }
                }

                if (_kindOfDataLoaded != KindOfDataLoaded.FromDataSource && isNetworkAvailable)
                {
                    // Read items from DataSource
                    var records = await DataSource.LoadData();
                    if (records != null)
                    {
                        _kindOfDataLoaded = KindOfDataLoaded.FromDataSource;
                        Items = new ObservableCollection<T>(records);
                        AppCache.AddItems(CacheKey, records);
                    }
                }
            }
            else
            {
                if (_kindOfDataLoaded == KindOfDataLoaded.Empty)
                {
                    var records = await DataSource.LoadData();
                    if (records != null)
                    {
                        Items = new ObservableCollection<T>(records);
                        _kindOfDataLoaded = KindOfDataLoaded.FromDataSource;
                    }
                }
            }

            ProgressBarVisibility = Visibility.Collapsed;
        }

        override public void LoadItem(string id)
        {
            T item = AppCache.GetItem<T>(id);
            if (item != null)
            {
                if (Items == null)
                {
                    Items = new ObservableCollection<T>(new T[] { item });
                    SelectedItem = item;
                }
                else
                {
                    Items.Clear();
                    Items.Add(item);
                    SelectedItem = item;
                }
            }
        }

        override public async Task RefreshItems()
        {
            ProgressBarVisibility = Visibility.Visible;

            // Read items from DataSource
            var records = await DataSource.Refresh();
            if (records != null)
            {
                _kindOfDataLoaded = KindOfDataLoaded.FromDataSource;
                Items = new ObservableCollection<T>(records);
                AppCache.AddItems(CacheKey, records);
            }

            ProgressBarVisibility = Visibility.Collapsed;
        }

        //
        //  Detail Commands
        //
        public ICommand ImageSelectedCommand
        {
            get { return new RelayCommand<string>(ImageTap); }
        }

        //
        // ICommands
        //
        public ICommand SpeakTextCommand
        {
            get { return new DelegateCommand(SpeakText); }
        }

        public ICommand PinToStartCommand
        {
            get { return new DelegateCommand(PinToStart); }
        }

        public ICommand GoToSourceCommand
        {
            get { return new DelegateCommand(GoToSource); }
        }

        public ICommand ShareItemCommand
        {
            get { return new DelegateCommand(ShareItem); }
        }

        public ICommand RefreshCommand
        {
            get { return new DelegateCommand(Refresh); }
        }

        //
        // Command implementation helpers
        //
        protected void SpeakText(params string[] propertyNames)
        {
            var currentItem = GetCurrentItem();
            if (currentItem != null)
            {
                SpeechServices.SpeakText(currentItem.GetValues(propertyNames));
            }
        }

        protected void PinToStart(string path, string titleToShare, string messageToShare, string imageToShare)
        {
            var currentItem = GetCurrentItem();
            if (currentItem != null)
            {
                if (String.IsNullOrEmpty(path))
                {
                    path = "MainPage";
                }
                var tileInfo = new TileServices.TileInfo()
                {
                    Id = currentItem.Id,
                    Title = GetBindingValue(titleToShare).Truncate(128),
                    BackTitle = GetBindingValue(titleToShare).Truncate(128),
                    BackContent = GetBindingValue(messageToShare).Truncate(128),
                    BackgroundImagePath = GetBindingValue(imageToShare),
                    BackBackgroundImagePath = GetBindingValue(imageToShare),
                    Count = 0
                };
                TileServices.PinTostart(path, tileInfo, SelectedItem);
            }
        }

        protected void GoToSource(string linkProperty)
        {
            var currentItem = GetCurrentItem();
            if (currentItem != null)
            {
                string url = GetBindingValue(linkProperty);
                if (!String.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    NavigationServices.NavigateTo(new Uri(url, UriKind.Absolute));
                }
            }
        }

        public void ShareItem(string titleToShare, string messageToShare, string linkToShare, string imageToShare)
        {
            var currentItem = GetCurrentItem();
            if (currentItem != null)
            {
                var shareServices = new ShareServices();
                shareServices.Share(GetBindingValue(titleToShare), GetBindingValue(messageToShare), GetBindingValue(linkToShare), GetBindingValue(imageToShare));
            }
        }

        private string GetBindingValue(string binding)
        {
            binding = binding ?? String.Empty;
            if (binding.StartsWith("{") && binding.EndsWith("}"))
            {
                var currentItem = GetCurrentItem();
                if (currentItem != null)
                {
                    string propertyName = binding.Substring(1, binding.Length - 2);
                    return currentItem.GetValue(propertyName);
                }
            }
            return binding;
        }

        protected T GetCurrentItem()
        {
            if (SelectedItem != null)
            {
                return SelectedItem;
            }
            if (Items != null && Items.Count > 0)
            {
                return Items[0];
            }
            return null;
        }

        virtual protected void NavigateToSelectedItem() { }

        abstract protected string CacheKey { get; }
        abstract protected IDataSource<T> CreateDataSource();

        #region KindOfDataLoaded
        private enum KindOfDataLoaded
        {
            Empty,
            FromCache,
            FromDataSource
        }

        private KindOfDataLoaded _kindOfDataLoaded = KindOfDataLoaded.Empty;
        #endregion
    }
}
