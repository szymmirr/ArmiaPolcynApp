using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Windows;

using Windows.System;

using Microsoft.Phone.Maps.Services;

namespace AppStudio.Services
{
    static public class NokiaMapsServices
    {
        static public void MapPosition(string address)
        {
            var geoQ = new GeocodeQuery();

            geoQ.GeoCoordinate = new GeoCoordinate(0, 0);
            geoQ.SearchTerm = address;
            geoQ.QueryCompleted += GeoQ_MapPositionQueryCompleted;
            geoQ.QueryAsync();
        }

        static public void HowToGet(string address)
        {
            var geoQ = new GeocodeQuery();

            geoQ.GeoCoordinate = new GeoCoordinate(0, 0);
            geoQ.SearchTerm = address;
            geoQ.QueryCompleted += GeoQ_HowToGetQueryCompleted;
            geoQ.QueryAsync();
        }

        static private async void GeoQ_MapPositionQueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Result.Any())
            {
                var result = e.Result.First();

                var latitude = result.GeoCoordinate.Latitude.ToString(CultureInfo.InvariantCulture);
                var longitude = result.GeoCoordinate.Longitude.ToString(CultureInfo.InvariantCulture);

                var uri = string.Concat("explore-maps://v2.0/show/place/?latlon=", latitude, ",", longitude);

                await Launcher.LaunchUriAsync(new Uri(uri));
            }
            else if (e.UserState is GeocodeQuery)
            {
                MessageBox.Show("No results for " + (e.UserState as GeocodeQuery).SearchTerm);
            }
        }

        static private async void GeoQ_HowToGetQueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Result.Any())
            {
                var result = e.Result.First();

                var latitude = result.GeoCoordinate.Latitude.ToString(CultureInfo.InvariantCulture);
                var longitude = result.GeoCoordinate.Longitude.ToString(CultureInfo.InvariantCulture);
                var uri = string.Concat("directions://v2.0/route/destination/?latlon=", latitude, ",", longitude);

                await Launcher.LaunchUriAsync(new Uri(uri));
            }
            else if (e.UserState is GeocodeQuery)
            {
                MessageBox.Show("No results for " + (e.UserState as GeocodeQuery).SearchTerm);
            }
        }
    }
}
