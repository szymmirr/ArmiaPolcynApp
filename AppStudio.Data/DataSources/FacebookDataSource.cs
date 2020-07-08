using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppStudio.Data
{
    public class FacebookDataSource : IDataSource<FacebookSchema>
    {
        private const string _userID = "452693068130565";

        private IEnumerable<FacebookSchema> _data = null;

        public FacebookDataSource()
        {
        }

        public async Task<IEnumerable<FacebookSchema>> LoadData()
        {
            if (_data == null)
            {
                try
                {
                    var facebookDataProvider = new FacebookDataProvider(_userID);
                    _data = await facebookDataProvider.Load();
                }
                catch (Exception ex)
                {
                    AppLogs.WriteError("FacebookDataSourceDataSource.LoadData", ex.ToString());
                }
            }
            return _data;
        }

        public async Task<IEnumerable<FacebookSchema>> Refresh()
        {
            _data = null;
            return await LoadData();
        }
    }
}
