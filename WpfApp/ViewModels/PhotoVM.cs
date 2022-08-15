using System;
using ReactiveUI;
using WpfApp.Model;
using static Microsoft.Requires;

namespace WpfApp.ViewModels;

public class PhotoVM : ReactiveObject
{
    public PhotoVM(Photo model)
    {
        NotNull(model, nameof(model));

        Title = model.Title;
        Photo = new Uri(model.Url);
        Thumbnail = new Uri(model.ThumbnailUrl);
    }

    public string Title { get; }

    public Uri Photo { get; }

    public Uri Thumbnail { get; }
}