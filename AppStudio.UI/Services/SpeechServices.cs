using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using Windows.Phone.Speech.Synthesis;

namespace AppStudio.Services
{
    /// <summary>
    /// Implementation of a Speech services.
    /// </summary>
    public class SpeechServices
    {
        const VoiceGender gender = VoiceGender.Female;

        static private SpeechSynthesizer _speech = null;

        static private SpeechSynthesizer Speech
        {
            get { return _speech ?? (_speech = CreateSpeech()); }
        }

        static private SpeechSynthesizer CreateSpeech()
        {
            var speech = new SpeechSynthesizer();
            var language = CultureInfo.CurrentCulture.ToString();
            var voices = InstalledVoices.All.Where(v => v.Language == language).OrderByDescending(v => v.Gender);
            speech.SetVoice(voices.FirstOrDefault(v => v.Gender == gender));
            return speech;
        }

        /// <summary>
        /// Converts a text into a speech and pronounces it.
        /// </summary>
        /// <param name="text">The text to be pronounced.</param>
        static public async void SpeakText(string text)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    await Speech.SpeakTextAsync(HtmlUtil.CleanHtml(text));
                }
            }
            catch (Exception ex)
            {
                AppLogs.WriteError("SpeechServices", ex);
            }
        }

        static public void Stop()
        {
            if (_speech != null)
            {
                _speech.CancelAll();
            }
        }
    }
}
