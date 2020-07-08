using System;
using System.Windows;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

using Nokia.Music;
using Nokia.Music.Tasks;
using Nokia.Music.Types;

namespace AppStudio.Services
{
    static public class NokiaMusicServices
    {
        /// <summary>
        ///     Launches show artist task.
        /// </summary>
        /// <param name="artist">The artist name</param>
        static public void LaunchArtist(string artist)
        {
            var task = new ShowArtistTask { ArtistName = artist };
            task.Show();
        }

        /// <summary>
        ///     Launches play artist mix task.
        /// </summary>
        /// <param name="artist">The artist name</param>
        static public void PlayArtistMix(string artist)
        {
            var task = new PlayMixTask { ArtistName = artist };
            task.Show();
        }

        /// <summary>
        ///     Launches search music task.
        /// </summary>
        /// <param name="searchTerm">The search terms</param>
        static public void LaunchSearch(string searchTerm)
        {
            var task = new MusicSearchTask { SearchTerms = searchTerm };
            task.Show();
        }
    }
}
