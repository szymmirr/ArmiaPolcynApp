using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text;

using SharpGIS;

namespace AppStudio.Data
{
    /// <summary>
    /// Rss data provider class, inherits from GZipWebClient.
    /// </summary>
    public class RssDataProvider : GZipWebClient
    {
        private Uri _uri;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url"></param>
        public RssDataProvider(string url)
        {
            _uri = new Uri(url);
        }

        /// <summary>
        /// Starts loading the feed and initializing the reader for the feed type.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RssSchema>> Load()
        {
            string xmlContent = await DownloadAsync(_uri);

            var doc = XDocument.Parse(xmlContent);
            if (doc.Declaration != null && doc.Declaration.Encoding != null)
            {
                if (!doc.Declaration.Encoding.EqualNoCase("UTF-8"))
                {
                    try
                    {
                        base.Encoding = Encoding.GetEncoding(doc.Declaration.Encoding);
                        xmlContent = await DownloadAsync(_uri);
                        doc = XDocument.Parse(xmlContent);
                    }
                    catch { }
                }
            }

            var type = BaseRssReader.GetFeedType(doc);

            BaseRssReader rssReader;
            if (type == RssType.Rss)
                rssReader = new RssReader();
            else
                rssReader = new AtomReader();

            return rssReader.LoadFeed(doc).Take(20);
        }

        private Task<string> DownloadAsync(Uri url)
        {
            var taskCompletionSrc = new TaskCompletionSource<string>();
            DownloadStringCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    taskCompletionSrc.TrySetException(e.Error);
                }
                else if (e.Cancelled)
                {
                    taskCompletionSrc.TrySetCanceled();
                }
                else
                {
                    taskCompletionSrc.TrySetResult(e.Result);
                }
            };
            DownloadStringAsync(url);
            return taskCompletionSrc.Task;
        }
    }
}
