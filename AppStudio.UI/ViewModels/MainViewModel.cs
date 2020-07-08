using System.Threading.Tasks;
using System.Windows.Input;

using AppStudio.Data;
using AppStudio.Services;

namespace AppStudio
{
    public class MainViewModels : BindableBase
    {
       private FacebookViewModel _facebookModel;
       private YouTubeViewModel _youTubeModel;
       private ArmiaWBinguViewModel _armiaWBinguModel;

        private ViewModelBase _selectedItem = null;

        public MainViewModels()
        {
            _selectedItem = FacebookModel;
        }
 
        public FacebookViewModel FacebookModel
        {
            get { return _facebookModel ?? (_facebookModel = new FacebookViewModel()); }
        }
 
        public YouTubeViewModel YouTubeModel
        {
            get { return _youTubeModel ?? (_youTubeModel = new YouTubeViewModel()); }
        }
 
        public ArmiaWBinguViewModel ArmiaWBinguModel
        {
            get { return _armiaWBinguModel ?? (_armiaWBinguModel = new ArmiaWBinguViewModel()); }
        }

        public void SetViewType(ViewTypes viewType)
        {
            FacebookModel.ViewType = viewType;
            YouTubeModel.ViewType = viewType;
            ArmiaWBinguModel.ViewType = viewType;
        }

        public ViewModelBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                UpdateAppBar();
            }
        }

        public bool IsAppBarVisible
        {
            get
            {
                if (SelectedItem == null || SelectedItem == FacebookModel)
                {
                    return true;
                }
                return SelectedItem != null ? SelectedItem.IsAppBarVisible : false;
            }
        }

        public bool IsLockScreenVisible
        {
            get { return SelectedItem == null || SelectedItem == FacebookModel; }
        }

        public bool IsAboutVisible
        {
            get { return SelectedItem == null || SelectedItem == FacebookModel; }
        }

        public void UpdateAppBar()
        {
            OnPropertyChanged("IsAppBarVisible");
            OnPropertyChanged("IsLockScreenVisible");
            OnPropertyChanged("IsAboutVisible");
        }

        /// <summary>
        /// Load ViewModel items asynchronous
        /// </summary>
        public async Task LoadData(bool isNetworkAvailable)
        {
            var loadTasks = new Task[]
            { 
                FacebookModel.LoadItems(isNetworkAvailable),
                YouTubeModel.LoadItems(isNetworkAvailable),
                ArmiaWBinguModel.LoadItems(isNetworkAvailable),
            };
            await Task.WhenAll(loadTasks);
        }

        //
        //  ViewModel command implementation
        //
        public ICommand AboutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateToPage("AboutThisAppPage");
                });
            }
        }

        public ICommand LockScreenCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    LockScreenServices.SetLockScreen("/Assets/LockScreenImage.png");
                });
            }
        }
    }
}
