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
            
            lvShowUser.ItemsSource = DataAccess.SelectData();
        }

        private static StorageFile file;
        //private static ObservableCollection<User> ListUser = new ObservableCollection<User>
        //{
        //    new User{name = "Pham Tien Bac1", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = "http://localhost/img/003%20-%20GRQdpKa.jpg"},
        //    new User{name = "Pham Tien Bac2", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"\..\Assets\LP-Wallpaper-PC-TECHRUM (3).jpg"},
        //    new User{name = "Pham Tien Bac3", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"D:\PHOTO\HÌNH NỀN 4K\003 - GRQdpKa.jpg"},
        //    new User{name = "Pham Tien Bac4", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"D:\PHOTO\HÌNH NỀN 4K\003 - GRQdpKa.jpg"},
        //    new User{name = "Pham Tien Bac5", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"D:\PHOTO\HÌNH NỀN 4K\003 - GRQdpKa.jpg"},
        //    new User{name = "Pham Tien Bac6", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"D:\PHOTO\HÌNH NỀN 4K\003 - GRQdpKa.jpg"},
        //    new User{name = "Pham Tien Bac7", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"D:\PHOTO\HÌNH NỀN 4K\003 - GRQdpKa.jpg"},
        //    new User{name = "Pham Tien Bac8", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"D:\PHOTO\HÌNH NỀN 4K\003 - GRQdpKa.jpg"},
        //    new User{name = "Pham Tien Bac9", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"D:\PHOTO\HÌNH NỀN 4K\003 - GRQdpKa.jpg"},
        //    new User{name = "Pham Tien Bac10", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"D:\PHOTO\HÌNH NỀN 4K\003 - GRQdpKa.jpg"},
        //    new User{name = "Pham Tien Bac11", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"D:\PHOTO\HÌNH NỀN 4K\003 - GRQdpKa.jpg"},
        //    new User{name = "Pham Tien Bac12", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"D:\PHOTO\HÌNH NỀN 4K\003 - GRQdpKa.jpg"},
        //    new User{name = "Pham Tien Bac13", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"D:\PHOTO\HÌNH NỀN 4K\003 - GRQdpKa.jpg"},
        //    new User{name = "Pham Tien Bac14", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"D:\PHOTO\HÌNH NỀN 4K\003 - GRQdpKa.jpg"},
        //    new User{name = "Pham Tien Bac15", email = "123@gmail.com", phone = "0986548996", address = "Ha Noi", avatar = @"D:\PHOTO\HÌNH NỀN 4K\003 - GRQdpKa.jpg"},
        //};

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

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                return true;
            }
            else
            {
                return false;
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
