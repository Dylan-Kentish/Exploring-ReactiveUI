using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData.Aggregation;
using DynamicData.Binding;
using ReactiveUI;
using WpfApp.Services;

namespace WpfApp.ViewModels
{
    public class MainWindowVM
    {
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;

        public MainWindowVM(IAlbumService albumService, IPhotoService photoService)
        {
            _albumService = albumService;
            _photoService = photoService;

            Albums = new ObservableCollection<AlbumVM>();

            GetAlbums = ReactiveCommand.CreateFromTask(GetAlbumsInternal);

            ClearAlbums = ReactiveCommand.Create(Albums.Clear, Albums.ToObservableChangeSet().IsNotEmpty());
        }

        private async Task GetAlbumsInternal()
        {
            var albums = await _albumService.GetAlbums();
            foreach (var album in albums)
            {
                var photos = await _photoService.GetAlbumPhotos(album.Id);
                Albums.Add(new AlbumVM(album, photos));
            }
        }

        public ICommand GetAlbums { get; }

        public ICommand ClearAlbums { get; }

        public ObservableCollection<AlbumVM> Albums { get; set; }
    }
}
