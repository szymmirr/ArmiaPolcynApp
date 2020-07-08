using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace AppStudio.Data
{
    public class AppCache
    {
        static public T GetItem<T>(string key) where T : BindableSchemaBase
        {
            string json = UserStorage.ReadTextFromFile(key);
            if (!String.IsNullOrEmpty(json))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                catch
                {
                    try
                    {
                        using (var userStorage = new UserStorage())
                        {
                            if (userStorage.FileExists(key))
                            {
                                userStorage.DeleteFile(key);
                            }
                        }
                    }
                    catch { }
                }
            }
            return null;
        }

        static public void AddItem<T>(string key, T item) where T : BindableSchemaBase
        {
            try
            {
                using (var userStorage = new UserStorage())
                {
                    AppLogs.WriteInfo("AddItem", key);
                    if (userStorage.FileExists(key))
                    {
                        userStorage.DeleteFile(key);
                    }
                    string json = JsonConvert.SerializeObject(item);
                    userStorage.WriteText(key, json);
                }
            }
            catch (Exception ex)
            {
                AppLogs.WriteError("AppCache.AddItem", ex);
            }
        }

        static private object _userStorageLock = new object();

        static public IEnumerable<T> GetItems<T>(string key) where T : BindableSchemaBase
        {
            string json = null;
            lock (_userStorageLock)
            {
                json = UserStorage.ReadTextFromFile(key);
            }

            if (!String.IsNullOrEmpty(json))
            {
                try
                {
                    IEnumerable<T> records = JsonConvert.DeserializeObject<IEnumerable<T>>(json);
                    return records;
                }
                catch
                {
                    try
                    {
                        lock (_userStorageLock)
                        {
                            using (var userStorage = new UserStorage())
                            {
                                if (userStorage.FileExists(key))
                                {
                                    userStorage.DeleteFile(key);
                                }
                            }
                        }
                    }
                    catch { }
                    return null;
                }
            }
            return null;
        }

        static public void AddItems<T>(string key, IEnumerable<T> items) where T : BindableSchemaBase
        {
            try
            {
                using (var userStorage = new UserStorage())
                {
                    AppLogs.WriteInfo("AddItems", key);
                    if (userStorage.FileExists(key))
                    {
                        userStorage.DeleteFile(key);
                    }
                    string json = JsonConvert.SerializeObject(items);
                    userStorage.WriteText(key, json);
                }
            }
            catch (Exception ex)
            {
                AppLogs.WriteError("AppCache.AddItems", ex);
            }
        }
    }
}
