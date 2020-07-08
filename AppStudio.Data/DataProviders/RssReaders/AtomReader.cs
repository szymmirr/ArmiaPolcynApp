﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace AppStudio.Data
{
    internal class AtomReader : BaseRssReader
    {
        /// <summary>
        /// Atom reader implementation to parse Atom content.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public override ObservableCollection<RssSchema> LoadFeed(XDocument doc)
        {
            try
            {
                var feed = new ObservableCollection<RssSchema>();

                if (doc.Root == null)
                    return feed;

                var items = doc.Root.Elements(doc.Root.GetDefaultNamespace() + "entry").Select(item => new RssSchema
                {
                    Title = item.GetSafeElementString("title").Trim(),
                    Summary = RssHelper.SanitizeString(Utility.DecodeHtml(GetItemSummary(item)).Trim().Truncate(500, true)),
                    Content = GetItemSummary(item),
                    ImageUrl = GetItemImage(item),
                    PublishDate = item.GetSafeElementDate("published"),
                    FeedUrl = item.GetLink("alternate"),
                }).ToList<RssSchema>();

                feed = new ObservableCollection<RssSchema>(items);

                return feed;
            }
            catch (Exception ex)
            {
                AppLogs.WriteError("AtomReader.LoadFeed", ex.Message);
                return null;
            }
        }

        private static string GetItemImage(XElement item)
        {
            if (!string.IsNullOrEmpty(item.GetSafeElementString("image")))
                return item.GetSafeElementString("image");

            return item.GetImage();
        }

        private static string GetItemSummary(XElement item)
        {
            var content = item.GetSafeElementString("description");
            if (string.IsNullOrEmpty(content))
            {
                content = item.GetSafeElementString("content");
            }
            if (string.IsNullOrEmpty(content))
            {
                content = item.GetSafeElementString("summary");
            }

            return content;
        }
    }
}
