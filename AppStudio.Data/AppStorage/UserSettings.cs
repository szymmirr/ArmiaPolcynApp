using System;
using System.IO.IsolatedStorage;

namespace AppStudio
{
    public class UserSettings
    {
        /// <summary>
        /// Gets the value associated with the specified key
        /// </summary>
        /// <param name="key">The key of the item to get or set.</param>
        /// <returns>
        /// The value associated with the specified key or null if the specified key is not found.
        /// </returns>
        static public object GetValue(string key)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                return IsolatedStorageSettings.ApplicationSettings[key];
            }
            return null;
        }

        /// <summary>
        /// Add or update a key/value pair in Isolated Storage Settings
        /// </summary>
        /// <param name="Key">The key for the entry to be stored.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>True if the value changed.</returns>
        static public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        /// <summary>
        /// Determines if the application settings dictionary contains the specified key
        /// </summary>
        /// <param name="Key">The key for the entry to be located.</param>
        /// <returns>True if the dictionary contains the specified key; otherwise, false.</returns>
        public static bool Contains(string key)
        {
            return IsolatedStorageSettings.ApplicationSettings.Contains(key);
        }

        /// <summary>
        /// Saves data written to the current Isolated Storage Settings object
        /// </summary>
        static public void Save()
        {
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        /// <summary>
        /// Remove a key/pair value from Isolated Storage Settings
        /// </summary>
        /// <param name="Key">The key for the entry to be removed.</param>
        static public void RemoveValue(string Key)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            // If the key exists
            if (settings.Contains(Key))
            {
                settings.Remove(Key);
            }
        }
    }
}
