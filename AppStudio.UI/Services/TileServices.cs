using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Phone.Shell;

using AppStudio.Data;

namespace AppStudio.Services
{
    public class TileServices
    {
        const string SHELLCONTENT_PATH = "/shared/shellcontent/";

        static public void PinTostart(string path, TileInfo tileInfo, BindableSchemaBase item)
        {
            AppCache.AddItem(tileInfo.Id, item);
            string url = String.Format("/Views/{0}.xaml?id={1}&DeepLink=true", path, tileInfo.Id);
            if (!TileExists(url))
            {
                CreateTile(url, tileInfo);
            }
            else
            {
                UpdateTile(url, tileInfo);
            }
        }

        static private bool TileExists(string url)
        {
            return ShellTile.ActiveTiles.Any(r => r.NavigationUri.ToString().EqualNoCase(url));
        }

        static private async void CreateTile(string url, TileInfo tileInfo)
        {
            await PopulateImages(tileInfo);
            var tileData = CreateTileData(tileInfo);
            ShellTile.Create(new Uri(url, UriKind.Relative), tileData);
        }

        static private async void UpdateTile(string url, TileInfo tileInfo)
        {
            await PopulateImages(tileInfo);
            var tileData = CreateTileData(tileInfo);
            var activeTile = ShellTile.ActiveTiles.FirstOrDefault(r => r.NavigationUri.ToString().EqualNoCase(url));
            if (activeTile != null)
            {
                activeTile.Update(tileData);
            }
        }

        static private StandardTileData CreateTileData(TileInfo tileInfo)
        {
            return new StandardTileData
            {
                Title = HtmlUtil.CleanHtml(tileInfo.Title),
                Count = tileInfo.Count,
                BackTitle = HtmlUtil.CleanHtml(tileInfo.BackTitle),
                BackContent = HtmlUtil.CleanHtml(tileInfo.BackContent),
                BackgroundImage = String.IsNullOrEmpty(tileInfo.BackgroundImagePath) ? null : new Uri(tileInfo.BackgroundImagePath, UriKind.RelativeOrAbsolute),
                BackBackgroundImage = String.IsNullOrEmpty(tileInfo.BackBackgroundImagePath) ? null : new Uri(tileInfo.BackBackgroundImagePath, UriKind.RelativeOrAbsolute)
            };
        }

        static private async Task PopulateImages(TileInfo tileInfo)
        {
            if (!String.IsNullOrEmpty(tileInfo.BackgroundImagePath))
            {
                tileInfo.BackgroundImagePath = await PopulateImage(tileInfo.BackgroundImagePath);
            }
            if (!String.IsNullOrEmpty(tileInfo.BackBackgroundImagePath))
            {
                tileInfo.BackBackgroundImagePath = await PopulateImage(tileInfo.BackBackgroundImagePath);
            }
        }

        static private async Task<string> PopulateImage(string url)
        {
            if (url.StartsWith("/"))
            {
                return url;
            }
            else
            {
                var uri = new Uri(url, UriKind.Absolute);
                string fileName = SHELLCONTENT_PATH + GetFileName(url);
                await DownloadImage(fileName, url);
                return "isostore:" + fileName;
            }
        }

        static private async Task DownloadImage(string fileName, string url)
        {
            var uri = new Uri(url);
            var request = WebRequest.CreateHttp(url);
            using (var response = await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null))
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var userStorage = new UserStorage())
                    {
                        using (var writer = userStorage.OpenFile(fileName, System.IO.FileMode.Create))
                        {
                            await stream.CopyToAsync(writer);
                            AppLogs.WriteInfo("DownloadImage", url);
                        }
                    }
                }
            }
        }

        static private string GetFileName(string str)
        {
            string fileName = "SCND";
            for (int n = 0; n < str.LastIndexOf('.'); n++)
            {
                char c = str[n];
                if (Char.IsLetterOrDigit(c))
                {
                    fileName += c;
                }
            }
            fileName = fileName + str.Substring(str.LastIndexOf('.'), 4);
            return fileName;
        }

        #region TileInfo
        public class TileInfo
        {
            /// <summary>
            /// Gets/Sets the identifier of the current notification showed in the tile.
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// Gets/Sets the title of the tile.
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// Gets/Sets the title showed in background of the tile.
            /// </summary>
            public string BackTitle { get; set; }
            /// <summary>
            /// Gets/Sets the content of the back side of the tile.
            /// </summary>
            public string BackContent { get; set; }
            /// <summary>
            /// Gets/Sets the background image of the tile.
            /// </summary>
            public string BackgroundImagePath { get; set; }
            /// <summary>
            /// Gets/Sets the background image of the back side of the tile.
            /// </summary>
            public string BackBackgroundImagePath { get; set; }
            /// <summary>
            /// Gets/Sets the counter for tile notifications.
            /// </summary>
            public int Count { get; set; }
        }
        #endregion
    }
}
