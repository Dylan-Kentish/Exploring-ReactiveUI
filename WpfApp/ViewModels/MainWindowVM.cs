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

        public MainWindowVM(IAlbumService albumService)
        {
            _albumService = albumService;

            Albums = new ObservableCollection<AlbumVM>();

            GetAlbums = ReactiveCommand.CreateFromTask(GetAlbumsInternal);

            ClearAlbums = ReactiveCommand.Create(Albums.Clear, Albums.ToObservableChangeSet().IsNotEmpty());
        }

        private async Task GetAlbumsInternal()
        {
            var albums = await _albumService.GetAlbums();

            var albumVMs = albums.Select(album => new AlbumVM(album));

            Albums.AddRange(albumVMs);
        }

        public ICommand GetAlbums { get; }

        public ICommand ClearAlbums { get; }

        public ObservableCollection<AlbumVM> Albums { get; set; }
    }
}
