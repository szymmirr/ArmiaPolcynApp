using System;
using System.Net;
using System.Windows.Input;

using AppStudio.Services;

namespace AppStudio
{
    public class ActionCommands
    {
        static public ICommand MailTo
        {
            get
            {
                return new RelayCommand<string>((param) =>
                {
                    if (!String.IsNullOrEmpty(param))
                    {
                        string url = String.Format("mailto:{0}", param);
                        NavigationServices.NavigateTo(new Uri(url));
                    }
                });
            }
        }

        static public ICommand CallToPhone
        {
            get
            {
                return new RelayCommand<string>((param) =>
                {
                    if (!String.IsNullOrEmpty(param))
                    {
                        string url = String.Format("callto:{0}", param);
                        NavigationServices.NavigateTo(new Uri(url));
                    }
                });
            }
        }

        static public ICommand MusicPlayArtistMix
        {
            get
            {
                return new RelayCommand<string>((param) =>
                {
                    if (!String.IsNullOrEmpty(param))
                    {
                        NokiaMusicServices.PlayArtistMix(param);
                    }
                });
            }
        }

        static public ICommand MusicLaunchSearch
        {
            get
            {
                return new RelayCommand<string>((param) =>
                {
                    if (!String.IsNullOrEmpty(param))
                    {
                        NokiaMusicServices.LaunchSearch(param);
                    }
                });
            }
        }

        static public ICommand MusicLaunchArtist
        {
            get
            {
                return new RelayCommand<string>((param) =>
                {
                    if (!String.IsNullOrEmpty(param))
                    {
                        NokiaMusicServices.LaunchArtist(param);
                    }
                });
            }
        }

        static public ICommand MapsPosition
        {
            get
            {
                return new RelayCommand<string>((param) =>
                {
                    if (!String.IsNullOrEmpty(param))
                    {
                        NokiaMapsServices.MapPosition(param);
                    }
                });
            }
        }

        static public ICommand MapsHowToGet
        {
            get
            {
                return new RelayCommand<string>((param) =>
                {
                    if (!String.IsNullOrEmpty(param))
                    {
                        NokiaMapsServices.HowToGet(param);
                    }
                });
            }
        }

        private static void NavigateTo(string protocol, string param)
        {
            if (!String.IsNullOrEmpty(param))
            {
                string url = String.Format("{0}:{1}", protocol, param);
                var uri = new Uri(url);
                NavigationServices.NavigateTo(uri);
            }
        }
    }
}
