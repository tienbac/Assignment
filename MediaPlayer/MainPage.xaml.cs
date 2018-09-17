using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using MediaPlayer.Entity;
using StorageFile = Windows.Storage.StorageFile;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MediaPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private bool isPlaying = false;
        public static ObservableCollection<FileProper> listFiles = new ObservableCollection<FileProper>();
        public static ObservableCollection<FileProper> listVideos = new ObservableCollection<FileProper>();

        private void TxtFilePath_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TextBox txtPath = sender as TextBox;
                if (txtPath != null)
                {
                    LoadMediaFromPath(txtPath.Text);
                }
            }
        }

        private void LoadMediaFromPath(string path)
        {
            try
            {
                Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
                MediaPlayer.Source = MediaSource.CreateFromUri(uri);
            }
            catch (Exception e)
            {

            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await SetLocalMedia();
        }

        async private System.Threading.Tasks.Task SetLocalMedia()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            //openPicker.FileTypeFilter.Add(".wmv");
            //openPicker.FileTypeFilter.Add(".mp4");
            //openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".mp3");
            openPicker.FileTypeFilter.Add(".flac");

            var file = await openPicker.PickMultipleFilesAsync();
            foreach (StorageFile i in file)
            {
                MusicProperties mp = await i.Properties.GetMusicPropertiesAsync();

                FileProper fp = new FileProper
                {
                    Album = mp.Album,
                    AlbumArtist = mp.AlbumArtist,
                    Artist = mp.Artist,
                    Duration = mp.Duration,
                    Publisher = mp.Publisher,
                    Rating = (int)mp.Rating,
                    StorageFile = i,
                    Subtitle = mp.Subtitle,
                    Name = i.Name,
                    Title = mp.Title,
                    TrackNumber = (int)mp.TrackNumber,
                    Year = (int)mp.Year
                };
                listFiles.Add(fp);
            }

            lvShowFile.ItemsSource = listFiles;
        }

        async private System.Threading.Tasks.Task SetLocalVideo()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wma");

            var file = await openPicker.PickMultipleFilesAsync();
            foreach (StorageFile i in file)
            {
                MusicProperties mp = await i.Properties.GetMusicPropertiesAsync();

                FileProper fp = new FileProper
                {
                    StorageFile = i,
                    Name = i.Name
                };
                listVideos.Add(fp);
            }

            lvShowVideo.ItemsSource = listVideos;
        }

        private async void PlayMedia(object sender, TappedRoutedEventArgs e)
        {
            Grid newGrid = (Grid)sender;
            FileProper fp = (FileProper) newGrid.Tag;
            StorageItemThumbnail thum = await fp.StorageFile.GetThumbnailAsync(ThumbnailMode.MusicView, 400);
            if (thum != null && thum.Type == ThumbnailType.Image)
            {
                var source = new BitmapImage();
                source.SetSource(thum);
                imgThumFile.Source = ImageBrush.ImageSource = source;
            }

            tbTitle.Text = fp.Title;
            tbAlbum.Text = fp.Album;
            tbArtist.Text = fp.Artist;
            MediaPlayer.Source = MediaSource.CreateFromStorageFile(fp.StorageFile);
            MediaPlayer.MediaPlayer.Play();
        }

        private async void VideoChoice(object sender, RoutedEventArgs e)
        {
            await SetLocalVideo();
        }


        private void PlayVideo(object sender, TappedRoutedEventArgs e)
        {
            Grid newGrid = (Grid) sender;
            FileProper fp = (FileProper) newGrid.Tag;
            VideoPlayer.Source = MediaSource.CreateFromStorageFile(fp.StorageFile);
            VideoPlayer.MediaPlayer.Play();
        }
    }
}
