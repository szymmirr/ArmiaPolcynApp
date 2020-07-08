using System;

namespace AppStudio.Data
{
    /// <summary>
    /// Implementation of the BingSchema class.
    /// </summary>
    public class BingSchema : BindableSchemaBase
    {
        private string _title;
        private string _summary;
        private string _link;
        private DateTime _published;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Summary
        {
            get { return _summary; }
            set { SetProperty(ref _summary, value); }
        }

        public string Link
        {
            get { return _link; }
            set { SetProperty(ref _link, value); }
        }

        public DateTime Published
        {
            get { return _published; }
            set { SetProperty(ref _published, value); }
        }

        public override string DefaultTitle
        {
            get { return Title; }
        }

        public override string DefaultSummary
        {
            get { return Summary; }
        }

        public override string DefaultImageUrl
        {
            get { return null; }
        }

        override public string GetValue(string fieldName)
        {
            if (!String.IsNullOrEmpty(fieldName))
            {
                switch (fieldName.ToLowerInvariant())
                {
                    case "id":
                        return String.Format("{0}", Id);
                    case "title":
                        return String.Format("{0}", Title);
                    case "summary":
                        return String.Format("{0}", Summary);
                    case "link":
                        return String.Format("{0}", Link);
                    case "published":
                        return String.Format("{0}", Published);
                    case "defaulttitle":
                        return String.Format("{0}", DefaultTitle);
                    case "defaultsummary":
                        return String.Format("{0}", DefaultSummary);
                    case "defaultimageurl":
                        return String.Format("{0}", DefaultImageUrl);
                    default:
                        break;
                }
            }
            return String.Empty;
        }
    }
}
