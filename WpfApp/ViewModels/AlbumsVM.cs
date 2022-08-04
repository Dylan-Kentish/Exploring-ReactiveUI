using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Binding;
using ReactiveUI;
using WpfApp.Services;
using static Microsoft.Requires;

namespace WpfApp.ViewModels
{
    public sealed class AlbumsVM : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable _disposable;
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;
        private readonly ObservableCollection<AlbumVM> _allAlbums;
        private readonly ReadOnlyObservableCollection<AlbumVM> _filteredAlbums;
        private string? _albumSearch;

        public AlbumsVM(IAlbumService albumService, IPhotoService photoService)
        {
            _albumService = NotNull(albumService, nameof(albumService));
            _photoService = NotNull(photoService, nameof(photoService));

            _disposable = new CompositeDisposable();

            _allAlbums = new ObservableCollection<AlbumVM>();

            GetAlbums = ReactiveCommand.CreateFromTask(GetAlbumsInternal);

            var observableChangeSet = _allAlbums.ToObservableChangeSet();

            ClearAlbums = ReactiveCommand.Create(_allAlbums.Clear, observableChangeSet.IsNotEmpty());

            var observableFilter = this.WhenAnyValue(x => x.AlbumSearch)
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.TaskpoolScheduler)
                .Select(query => query?.Trim())
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(MakeFilter);

            var myDerivedCache = observableChangeSet
                .Filter(observableFilter)
                .AsObservableList();

            _ = myDerivedCache.DisposeWith(_disposable);

            _ = myDerivedCache
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _filteredAlbums)
                .Subscribe()
                .DisposeWith(_disposable);
        }

        private async Task GetAlbumsInternal()
        {
            var albums = await _albumService.GetAlbums();
            foreach (var album in albums)
            {
                var photos = await _photoService.GetAlbumPhotos(album.Id);
                _allAlbums.Add(new AlbumVM(album, photos));
            }
        }

        public ICommand GetAlbums { get; }

        public ICommand ClearAlbums { get; }

        public ReadOnlyObservableCollection<AlbumVM> Albums => _filteredAlbums;

        public string? AlbumSearch
        {
            get => _albumSearch;
            set => this.RaiseAndSetIfChanged(ref _albumSearch, value);
        }

        private static Func<AlbumVM, bool> MakeFilter(string? filter)
        {
            return album => string.IsNullOrWhiteSpace(filter) || album.Title.Contains(filter);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
