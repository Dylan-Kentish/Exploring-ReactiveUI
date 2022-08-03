using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft;
using WpfApp.Model;

namespace WpfApp.ViewModels
{
    public class PhotoVM
    {
        public PhotoVM(Photo model)
        {
            Requires.NotNull(model, nameof(model));

            Photo = new BitmapImage(
                new Uri(model.Url));
            Thumbnail = new BitmapImage(
                new Uri(model.ThumbnailUrl));
        }

        public ImageSource Photo { get; }
        public ImageSource Thumbnail { get; }
    }
}
