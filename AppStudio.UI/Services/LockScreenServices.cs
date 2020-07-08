using System;

namespace AppStudio.Services
{
    /// <summary>
    /// Implementation of a Lock Screen service.
    /// </summary>
    static public class LockScreenServices
    {
        /// <summary>
        /// Sets the Lock Screen for the application.
        /// </summary>
        /// <param name="lockKey">The lock key.</param>
        static public async void SetLockScreen(string lockKey)
        {
            Uri imageUri = null;

            if (!string.IsNullOrEmpty(lockKey))
            {
                imageUri = new Uri("ms-appx:///" + lockKey, UriKind.Absolute);

                if (!Windows.Phone.System.UserProfile.LockScreenManager.IsProvidedByCurrentApplication)
                {
                    //Ask for permissions.
                    var permission = await Windows.Phone.System.UserProfile.LockScreenManager.RequestAccessAsync();

                    if (permission == Windows.Phone.System.UserProfile.LockScreenRequestResult.Denied)
                    {
                        //Permission is denied.
                        return;
                    }
                }

                Windows.Phone.System.UserProfile.LockScreen.SetImageUri(imageUri);
            }
        }
    }
}
