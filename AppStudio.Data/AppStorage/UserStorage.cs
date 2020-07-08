using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;

namespace AppStudio
{
    public class UserStorage : IDisposable
    {
        static private object _userStoreLock = new object();

        private IsolatedStorageFile _userStore = null;

        public UserStorage()
        {
            _userStore = IsolatedStorageFile.GetUserStoreForApplication();
        }

        public void EnsureDirectory(string path)
        {
            lock (_userStoreLock)
            {
                if (!_userStore.DirectoryExists(path))
                {
                    _userStore.CreateDirectory(path);
                }
            }
        }

        public string[] GetFiles(string path)
        {
            EnsureDirectory(path);
            return _userStore.GetFileNames(path + "/*");
        }

        public IsolatedStorageFileStream OpenFile(string path, FileMode mode)
        {
            return _userStore.OpenFile(path, mode);
        }

        public void WriteText(string fileName, string text)
        {
            lock (_userStoreLock)
            {
                using (IsolatedStorageFileStream fileStream = _userStore.OpenFile(fileName, FileMode.Create))
                {
                    using (var writer = new StreamWriter(fileStream))
                    {
                        writer.WriteLine(text);
                    }
                }
            }
        }

        public void AppendLine(string fileName, string line)
        {
            lock (_userStoreLock)
            {
                using (IsolatedStorageFileStream fileStream = _userStore.OpenFile(fileName, FileMode.Append))
                {
                    using (var writer = new StreamWriter(fileStream))
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }

        public string ReadText(string fileName)
        {
            lock (_userStoreLock)
            {
                using (IsolatedStorageFileStream fileStream = _userStore.OpenFile(fileName, FileMode.Open))
                {
                    using (var reader = new StreamReader(fileStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public bool FileExists(string fileName)
        {
            return _userStore.FileExists(fileName);
        }

        public void DeleteFile(string fileName)
        {
            _userStore.DeleteFile(fileName);
        }

        //
        // Static Methods
        //

        static public string[] GetFileNames(string path)
        {
            using (var userStorage = new UserStorage())
            {
                return userStorage.GetFiles(path);
            }
        }

        static public void WriteTextToFile(string fileName, string text)
        {
            using (var userStorage = new UserStorage())
            {
                userStorage.WriteText(fileName, text);
            }
        }

        static public void WriteLineToFile(string fileName, string line)
        {
            using (var userStorage = new UserStorage())
            {
                userStorage.AppendLine(fileName, line);
            }
        }

        static public string ReadTextFromFile(string fileName)
        {
            try
            {
                using (var userStorage = new UserStorage())
                {
                    lock (_userStoreLock)
                    {
                        if (userStorage.FileExists(fileName))
                        {
                            return userStorage.ReadText(fileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogs.WriteError("UserStorage.ReadTextFromFile", ex);
            }
            return String.Empty;
        }

        static public void Delete(string fileName)
        {
            using (var userStorage = new UserStorage())
            {
                userStorage.DeleteFile(fileName);
            }
        }

        #region Dispose
        public void Dispose()
        {
            if (_userStore != null)
            {
                _userStore.Dispose();
                _userStore = null;
            }
        }
        #endregion
    }
}
