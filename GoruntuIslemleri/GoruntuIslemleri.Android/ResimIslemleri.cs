using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace GoruntuIslemleri.Droid
{
    public class ResimIslemleri : IResimIslemleri
    {
        public ResimBoyutlari ResimBoyutlariAl(string dosyaYolu)
        {
            ResimBoyutlari resimBoyutlari = new ResimBoyutlari();
            var task = new TaskCompletionSource<double>();
            var cachedImage = new CachedImage()
            {
                Source = ImageSource.FromFile(dosyaYolu),
                CacheType = FFImageLoading.Cache.CacheType.Memory

            };


            cachedImage.Success += (sender, e) =>
            {
                var h = e.ImageInformation.OriginalHeight;
                resimBoyutlari.Yukseklik = h;
                var w = e.ImageInformation.OriginalWidth;
                resimBoyutlari.Genislik = w;
            };

            cachedImage.Error += (sender, e) => {
                task.TrySetException(e.Exception);
                //DisplayAlert("Hata oluştu", e.Exception.Message.ToString(), "Tamam");
            };

            return resimBoyutlari;

        }
    }
}