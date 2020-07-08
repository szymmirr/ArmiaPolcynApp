using SharpGIS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Data
{
    public class FacebookDataProvider
    {
        private const string USER_RSS_URL = "https://www.facebook.com/feeds/page.php?id={0}&format=rss20";
        private Uri _uri;

        public FacebookDataProvider(string userID)
        {
            _uri = new Uri(string.Format(USER_RSS_URL, userID));
        }

        public async Task<ObservableCollection<FacebookSchema>> Load()
        {
            var reader = new RssDataProvider(_uri.ToString());
            var rssSchemaList = await reader.Load();
            var result = new ObservableCollection<FacebookSchema>
                (
                    rssSchemaList.Select(rss => new FacebookSchema()
                    {
                        Author = rss.Author,
                        Content = rss.Content,
                        FeedUrl = rss.FeedUrl,
                        Id = rss.Id,
                        ImageUrl = rss.ImageUrl,
                        PublishDate = rss.PublishDate,
                        Summary = rss.Summary,
                        Title = rss.Title
                    })
                    .OrderByDescending(f => f.PublishDate)
                );

            return result;
        }
    }
}
