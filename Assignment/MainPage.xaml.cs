using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Assignment.Entity;
using Assignment.Model;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Assignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
            lvShowUser.ItemsSource  = DataAccess.SelectData();
            
           WriteFile();
        }

        private static StorageFile file;
        private static ObservableCollection<User> ListUserW;
        

        private async void TakeAPicture(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI camera = new CameraCaptureUI();
            camera.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            camera.PhotoSettings.CroppedSizeInPixels = new Size(300, 300);

            file = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (file == null)
            {
                return;
            }

            StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("ImagesProfile", CreationCollisionOption.OpenIfExists);
            await file.CopyAsync(storageFolder, "ProfileImg.jpg", NameCollisionOption.ReplaceExisting);

            BitmapImage sourImage = await LoadImage(file);
            tbFileName.Text = "Photo name : " + file.Name;
            imgProfile.Source = sourImage;
        }

        private async void SelectImage(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                tbFileName.Text = "Photo name : " + file.Name;
                BitmapImage source = await LoadImage(file);
                Debug.WriteLine(file.Path);
                imgProfile.Source = source;
            }
            else
            {
                tbFileName.Text = "Operation cancelled.";
            }
        }

        // C:\Users\[userlaptop]\AppData\Local\Packages\1204ec44-81f3-4207-b563-ac8b36dc2ef6_e4ntbfprrfm0g\LocalState
        // Co the su dung Tim Kiem Everything de tim nhanh hon
        public async void WriteFile()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file1 = await storageFolder.CreateFileAsync("JSONFILE.json",
                CreationCollisionOption.OpenIfExists);

            await FileIO.WriteTextAsync(file1, "DATA WITH JSON\r\n");

            // Append a list of strings, one per line, to the file
            ListUserW = DataAccess.SelectData();
            string json = JsonConvert.SerializeObject(ListUserW.ToArray());
            var listOfStrings = new List<string> { json };
            Debug.WriteLine(json);
            await FileIO.AppendLinesAsync(file1, listOfStrings);
        }

        // Load Image
        public static async Task<BitmapImage> LoadImage(StorageFile file)
        {
            BitmapImage source = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream) await file.OpenAsync(FileAccessMode.Read);

            source.SetSource(stream);
            return source;
        }


        private void AddNew(object sender, RoutedEventArgs e)
        {
            var name = txtName.Text;
            var email = txtEmail.Text;
            var phone = txtPhone.Text;
            var address = txtAddress.Text;
            var avatar = file.Path;

            User user = new User{name = name, email = email, phone = phone, address = address, avatar = avatar};
            var json = JsonConvert.SerializeObject(user);
            Debug.WriteLine(json);
            DataAccess.AddData(user);
            RefreshData();
        }

        private async void Show(object sender, TappedRoutedEventArgs e)
        {
            int a = 0;
            stpShowInfor.Visibility = (Visibility)a;
            Grid newGrid = (Grid) sender;

            User newUser = (User) newGrid.Tag;
            tbName.Text = newUser.name;
            tbEmail.Text = newUser.email;
            tbPhone.Text = newUser.phone;
            tbAddress.Text = newUser.address;
            tbFilePath.Text = newUser.avatar;

            var img = newUser.avatar;
            BitmapImage source = new BitmapImage(new Uri(img, UriKind.RelativeOrAbsolute));
            imgAvatar.Source = source;
        }

        public void RefreshData()
        {
            txtName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            tbFileName.Text = "";
            
        }

        public static async Task<BitmapImage> GetImg(string path)
        {
            BitmapImage source = new BitmapImage();
            //using (var stream = new IsolatedStorageFileStream(path, FileMode.Open, FileAccess.Read, IsolatedStorageFile.GetUserStoreForApplication()))
            //{
            //    await source.SetSourceAsync(stream.AsRandomAccessStream());
            //}

            var file1 = await ApplicationData.Current.LocalFolder.CreateFileAsync(path,
                CreationCollisionOption.OpenIfExists);
            using (var stream = await file1.OpenReadAsync())
            {
                await source.SetSourceAsync(stream);
            }

            return source;
        }

        private void HideInfor(object sender, DoubleTappedRoutedEventArgs e)
        {
            int b = 1;
            stpShowInfor.Visibility = (Visibility) b;
        }

        private void AbtnSearch(object sender, RoutedEventArgs e)
        {
            string text = txtKeyWord.Text;
            //lvShowUser.ItemsSource = DataAccess.FindDataPhone(text);
            ObservableCollection<User> list;
            if (text.Length > 0 )
            {
                string substring = text.Substring(0, 2);
                if (String.Compare(substring, "09", true) == 0 || String.Compare(substring, "01", true) == 0)
                {
                    list = DataAccess.FindDataPhone(text);
                    lvShowUser.ItemsSource = list;
                }

                bool check = IsValidEmail(text);
                if (check)
                {
                    list = DataAccess.FindDataEmail(text);
                    lvShowUser.ItemsSource = list;
                }

                bool check2 = IsValidFullName(text);
                if (check2)
                {
                    list = DataAccess.FindDataNameKey(text);
                    lvShowUser.ItemsSource = list;
                }
            }
            else
            {
                lvShowUser.ItemsSource = DataAccess.SelectData();
            }
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsValidFullName(string inputN)
        {
            
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            var hasNumber = new Regex(@"[0-9]+");
            if (string.IsNullOrWhiteSpace(inputN))
            {
                
                return false;
            }
            else if (hasNumber.IsMatch(inputN))
            {
               
                return false;
            }
            else if (hasSymbols.IsMatch(inputN))
            {
                
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
