using AppStudio.Resources;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Windows.Networking.Proximity;

namespace AppStudio.Services
{
    /// <summary>
    /// Implementation of the Proximity service.
    /// </summary>
    public sealed class ProximityServices
    {
        private ProximityDevice _device = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProximityServices()
        {
            _device = ProximityDevice.GetDefault();
        }

        /// <summary>
        /// THis property returns true if the device has proximite capabilities.
        /// </summary>
        public bool IsProximityAvailable
        {
            get { return _device != null; }
        }

        /// <summary>
        /// Executes the Proximity service.
        /// </summary>
        /// <param name="value">The value to share</param>
        public void ShareUri(string value)
        {
            var cancelPublishUriMessageDialog = GetCancelTapSendMesssage();

            // Make sure NFC is supported
            if (_device != null)
            {
                var uri = new Uri(value, UriKind.RelativeOrAbsolute);
                long Id = _device.PublishUriMessage(uri, (sender, messageId) =>
                {
                    _device.StopPublishingMessage(messageId);
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        cancelPublishUriMessageDialog.Dismiss();
                    });
                });
                cancelPublishUriMessageDialog.Dismissed += (sender, e) =>
                {
                    _device.StopPublishingMessage(Id);
                };
                cancelPublishUriMessageDialog.Show();
            }
        }

        private static CustomMessageBox GetCancelTapSendMesssage()
        {
            var tapSendImage = new Image
            {
                Source = new BitmapImage(new Uri("Assets/tap+send.png", UriKind.Relative)),
                Stretch = System.Windows.Media.Stretch.None
            };
            var cancelPublishUriMessageDialog = new CustomMessageBox
            {
                Title = AppResources.TapSend.ToUpperInvariant(),
                Message = AppResources.TapSendMessage,
                Content = tapSendImage,
                IsLeftButtonEnabled = true,
                LeftButtonContent = "cancel",
                IsRightButtonEnabled = false,
                IsFullScreen = true
            };
            return cancelPublishUriMessageDialog;
        }
    }
}
