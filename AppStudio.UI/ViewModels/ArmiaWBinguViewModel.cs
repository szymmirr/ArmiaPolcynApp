using System;
using System.Windows;
using System.Windows.Input;

using AppStudio.Services;

namespace AppStudio.Data
{
    public class ArmiaWBinguViewModel : ViewModelBase<BingSchema>
    {
        override protected string CacheKey
        {
            get { return "ArmiaWBinguDataSource"; }
        }

        override protected IDataSource<BingSchema> CreateDataSource()
        {
            return new ArmiaWBinguDataSource(); // BingDataSource
        }

        override public bool IsRefreshVisible
        {
            get { return ViewType == ViewTypes.List; }
        }

        override protected void NavigateToSelectedItem()
        {
            var currentItem = GetCurrentItem();
            if (currentItem != null)
            {
                NavigationServices.NavigateTo(new Uri(currentItem.GetValue("Link")));
            }
        }
    }
}
