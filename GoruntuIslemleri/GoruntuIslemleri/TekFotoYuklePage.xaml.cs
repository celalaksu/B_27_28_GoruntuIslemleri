using FFImageLoading.Forms;
using Microsoft.WindowsAzure.Storage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoruntuIslemleri
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TekFotoYuklePage : ContentPage
    {
        public TekFotoYuklePage()
        {
            InitializeComponent();
        }

        private async void fotoCekButton_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Kamera Sorunu", "Kamera bulunamadı :(", "Tamam");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Test",
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Rear
            });

            if (file == null)
                return;

            await DisplayAlert("Dosya kayıt yolu", file.Path, "Tamam");

            resimImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

            /*
            var mediaFile = file as MediaFile;
            var dosyaYolu = file.Path.ToString();

            //ResimBoyutlariAl(mediaFile);
            IResimIslemleri ri = DependencyService.Resolve<IResimIslemleri>();
            ResimBoyutlari rb = ri.ResimBoyutlariAl(dosyaYolu);
            */
        }
        /*
        void ResimBoyutlariAl(MediaFile mediaFile)
        {
            var task = new TaskCompletionSource<double>();
            var cachedImage = new CachedImage()
            {
                Source =ImageSource.FromFile(mediaFile.Path),
                CacheType = FFImageLoading.Cache.CacheType.Memory
                
            };
            

            cachedImage.Success += (sender, e) =>
            {
                var h = e.ImageInformation.OriginalHeight;
                yükseklikLabel.Text = h.ToString();
                var w = e.ImageInformation.OriginalWidth;
                genislikLabel.Text = w.ToString();
            };

            cachedImage.Error += (sender, e) => {
                task.TrySetException(e.Exception);
                DisplayAlert("Hata oluştu", e.Exception.Message.ToString(), "Tamam");
            };


        }

    */
        private async void fotoSecButton_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Fotograf Hatası", "Fotografalara erişim izni yok.", "Tamam");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,

            });

            if (file == null)
                return;
            FotoYukleAzureDepolama(file.GetStream());

            resimImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

            
        }

        private async void FotoYukleAzureDepolama(Stream streamResim)
        {
            var depolamaHesabi = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=depolamahesabinotdefteri;AccountKey=B0/8v8c3oZx1ucTRvcCeDHPQw2870D7V43nrAZUsA2JrPOyy6J2d7HUpJnooNIy08WAGc+LZtDIfY1wAJOsm4g==;EndpointSuffix=core.windows.net");
            var istemci = depolamaHesabi.CreateCloudBlobClient();
            var bolumContainer = istemci.GetContainerReference("resimcontainer");
            await bolumContainer.CreateIfNotExistsAsync();

            var dosyaAdi = Guid.NewGuid().ToString();
            var dosyaBlob = bolumContainer.GetBlockBlobReference($"{dosyaAdi}.jpg");
            await dosyaBlob.UploadFromStreamAsync(streamResim);

        }

        private async void videoCekButton_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                await DisplayAlert("Kamera Sorunu", "Kamera bulunamadı", "Tamam");
                return;
            }

            var file = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions
            {
                Name = "video.mp4",
                Directory = "DefaultVideos",
            });

            if (file == null)
                return;

            await DisplayAlert("Video Kaydedildi", "Konumu : " + file.Path, "Tamam");

            file.Dispose();

        }

        private async void videoSecButton_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickVideoSupported)
            {
                await DisplayAlert("Video Yükleme Hatası", "Videolara erişim için izin verilmedi", "Tamam");
                return;
            }
            var file = await CrossMedia.Current.PickVideoAsync();

            if (file == null)
                return;

            await DisplayAlert("Video seçildi", "Konumu : " + file.Path, "Tamam");
            file.Dispose();
        }
    }
}