using System;
using System.Net;

using Windows.System;

using AppStudio.Data;

namespace AppStudio.Services
{
    public class NavigationServices
    {
        static public ViewModelBase CurrentViewModel { get; set; }

        static public void NavigateToPage(string path, string queryString = "")
        {
            if (!String.IsNullOrEmpty(path))
            {
                string fullPath = String.Format("/Views/{0}.xaml?{1}", path, queryString);
                App.RootFrame.Navigate(new Uri(fullPath, UriKind.Relative));
            }
        }

        static async public void NavigateTo(Uri uri)
        {
            if (uri != null)
            {
                if (uri.IsAbsoluteUri)
                {
                    string query = HttpUtility.UrlDecode(uri.Host + uri.PathAndQuery);
                    switch (uri.Scheme.ToLowerInvariant())
                    {
                        case "nokia-maps-position":
                            NokiaMapsServices.MapPosition(query);
                            break;
                        case "nokia-maps-how-to-get":
                            NokiaMapsServices.HowToGet(query);
                            break;
                        case "nokia-music-artist":
                            NokiaMusicServices.LaunchArtist(query);
                            break;
                        case "nokia-music-play-artist-mix":
                            NokiaMusicServices.PlayArtistMix(query);
                            break;
                        case "nokia-music-search":
                            NokiaMusicServices.LaunchSearch(query);
                            break;
                        default:
                            await Launcher.LaunchUriAsync(uri);
                            break;
                    }
                }
                else
                {
                    App.RootFrame.Navigate(uri);
                }
            }
        }
    }
}
