using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MediaPlayer.Entity
{
    public class FileProper
    {
        public StorageFile StorageFile { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string AlbumArtist { get; set; }
        public int Year { get; set; }
        public TimeSpan Duration { get; set; }
        public string Publisher { get; set; }
        public string Subtitle { get; set; }
        public int Rating { get; set; }
        public int TrackNumber { get; set; }
        public BitmapImage Image { get; set; }
    }
}
