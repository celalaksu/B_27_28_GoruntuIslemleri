using FFImageLoading.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoruntuIslemleri
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CokluSecimPage : ContentPage
    {
        ObservableCollection<MediaFile> resimDosyalari = new ObservableCollection<MediaFile>(); //using System.Collections.ObjectModel;
                                                                                                //using Plugin.Media.Abstractions; MEDİAFİLE SINIFI İÇİN



        public CokluSecimPage()
        {
            InitializeComponent();
            //resimDosyalari.CollectionChanged += Files_CollectionChanged;
        }

        private async void fotolariSecButton_Clicked(object sender, EventArgs e)
        {
            //await CrossMedia.Current.Initialize();
            resimDosyalari.Clear();
            resimListesiSL.Children.Clear();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var picked = await CrossMedia.Current.PickPhotosAsync();

            if (picked == null)
                return;
            foreach (var file in picked)
                resimDosyalari.Add(file);

            FotolariGoster(resimDosyalari);
        }

        void FotolariGoster(ObservableCollection<MediaFile> resimDosyalari)
        {
            if (resimDosyalari.Count == 0)
            {
                resimListesiSL.Children.Clear();
                return;
            }
            foreach (MediaFile mediaFile in resimDosyalari)
            {
                //var image = new Image { WidthRequest = 300, HeightRequest = 300, Aspect = Aspect.AspectFit };
                //image.Source = ImageSource.FromFile(mediaFile.Path);
                /*image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });*/
                //ImageList.Children.Add(image);



                var resim = new CachedImage { WidthRequest = 300, HeightRequest = 300, Aspect = Aspect.AspectFit };
                //using FFImageLoading.Forms; paketi CachedImage için gerekli
                resim.Source = ImageSource.FromFile(mediaFile.Path);

                //ImageList.Children.Add(image2);

                var layout = new StackLayout();
                var layout2 = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                layout.Children.Add(resim);
                layout.Children.Add(layout2);

                resimListesiSL.Children.Add(layout);

                resim.Success += (sender, e) =>
                {
                    var w = e.ImageInformation.OriginalWidth;
                    var genislikLabel = new Label { Text = "Genişlik : " + w.ToString() };
                    layout2.Children.Add(genislikLabel);

                    var h = e.ImageInformation.OriginalHeight;
                    var yukseklikLabel = new Label { Text = "Yükseklik : " + h.ToString() };
                    layout2.Children.Add(yukseklikLabel);
                };
            }
        }


        /*
        private void Files_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (resimDosyalari.Count == 0)
            {
                resimListesiSL.Children.Clear();
                return;
            }
            if (e.NewItems.Count == 0)
                return;

            var file = e.NewItems[0] as MediaFile;
            var image = new Image { WidthRequest = 300, HeightRequest = 300, Aspect = Aspect.AspectFit };
            image.Source = ImageSource.FromFile(file.Path);
            image.Source = ImageSource.FromStream(() =>
			{
				var stream = file.GetStream();
				return stream;
			});
            //ImageList.Children.Add(image);
            var image2 = new CachedImage { WidthRequest = 300, HeightRequest = 300, Aspect = Aspect.AspectFit };
            //using FFImageLoading.Forms; paketi CachedImage için gerekli
            image2.Source = ImageSource.FromFile(file.Path);           
            resimListesiSL.Children.Add(image2);            
            var underlineLabel = new Label { Text = file.Path, TextDecorations = TextDecorations.Underline };
            resimListesiSL.Children.Add(underlineLabel);   
        } 
    */

    }
}