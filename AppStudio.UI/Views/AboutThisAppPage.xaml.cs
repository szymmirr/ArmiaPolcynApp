using System;

using Microsoft.Phone.Controls;

using AppStudio.Data;

namespace AppStudio
{
    public partial class AboutThisAppPage : PhoneApplicationPage
    {
        public AboutThisAppPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        public AboutThisAppViewModel AboutModel
        {
            get { return AboutThisAppViewModel.Current; }
        }
    }
}
