using System.Collections.Generic;
using System.Linq;
using Microsoft;
using WpfApp.Model;

namespace WpfApp.ViewModels
{
    public class AlbumVM
    {
        private readonly Album _model;
        private readonly IEnumerable<Photo> _photos;

        public AlbumVM(Album model, IEnumerable<Photo> photos)
        {
            Requires.NotNull(model, nameof(model));
            Requires.NotNull(photos, nameof(photos));
            Requires.Argument(photos.Any(), nameof(photos), "photos is empty");

            _model = model;
            _photos = photos;
            Thumbnail = new PhotoVM(_photos.First());
        }

        public string Title => _model.Title;
        public PhotoVM Thumbnail { get; } 
    }
}
