using System;
using System.Windows;
using System.Windows.Input;

using AppStudio.Services;

namespace AppStudio.Data
{
    public class FacebookViewModel : ViewModelBase<FacebookSchema>
    {
        override protected string CacheKey
        {
            get { return "FacebookDataSource"; }
        }

        override protected IDataSource<FacebookSchema> CreateDataSource()
        {
            return new FacebookDataSource(); // FacebookDataSource
        }

        override public bool IsRefreshVisible
        {
            get { return ViewType == ViewTypes.List; }
        }

        override protected void NavigateToSelectedItem()
        {
            NavigationServices.NavigateToPage("FacebookDetail");
        }
    }
}
