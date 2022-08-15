using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp.Model;
using static Microsoft.Requires;

namespace WpfApp.ViewModels;

public class PhotoVM
{
    public PhotoVM(Photo model)
    {
        NotNull(model, nameof(model));

        Title = model.Title;
        Photo = new BitmapImage(
            new Uri(model.Url));
        Thumbnail = new BitmapImage(
            new Uri(model.ThumbnailUrl));
    }

    public string Title { get; }
    public ImageSource Photo { get; }
    public ImageSource Thumbnail { get; }
}