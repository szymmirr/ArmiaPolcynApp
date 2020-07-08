using System;
using System.Xml.Linq;
using System.Windows.Input;

using AppStudio.Services;

namespace AppStudio.Data
{
    public class AboutThisAppViewModel
    {
        static private AboutThisAppViewModel _current = null;
        static private ShareServices _shareService = new ShareServices();

        static public AboutThisAppViewModel Current
        {
            get { return _current ?? (_current = new AboutThisAppViewModel()); }
        }

        public string DeveloperName
        {
            get
            {
                return XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Author").Value;
            }
        }

        public string AppVersion
        {
            get
            {
                return XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
            }
        }

        public string AboutText
        {
            get
            {
                return "Oficjalna aplikacja Windows Phone rosnącego w siłę ugrupowania założonego w 2011r" +
    ". w Zespole Szkół Ponadgimnazjalnych nr 1 w Pile.";
            }
        }

        public string LicenseText
        {
            get
            {
                return @"<p><b>Important read carefully</b></p>" +
                          @"This End-User License Agreement (""EULA"") is a legal agreement " +
                          @"between you (either an individual or a single entity) and " +
                          @"Microsoft Corporation for the Microsoft software that accompanies " +
                          @"this EULA, which includes associated media and Microsoft Internet-based " +
                          @"services (""Software""). <br/>" +
                          @"An amendment or addendum to this EULA may accompany the Software. " +
                          @"<p><b>You agree to be bound by the terms of this EULA by installing, copying, " +
                          @"or using the software. If you do not agree, do not install, copy, or use the " +
                          @"software; you may return it to your place of purchase for a full refund, if applicable.</b></p>";
            }
        }

        public ICommand ShareAppCommand
        {
            get
            {
                return new RelayCommand<string>((src) =>
                {
                    string appUri = String.Format("http://xap.winappstudio.com/Job/GetXap?ticket={0}", "141580.pvqysj");
                    if (_shareService.AppExistInMarketPlace())
                    {
                        appUri = Windows.ApplicationModel.Store.CurrentApp.LinkUri.AbsoluteUri;
                    }
                    _shareService.Share("app", "message", appUri, string.Empty);
                });
            }
        }
    }
}
