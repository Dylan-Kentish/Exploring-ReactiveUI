using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using WpfApp.Services;
using static Microsoft.Requires;

namespace WpfApp.ViewModels
{
    public sealed class AlbumsVM : ReactiveObject, IDisposable, IValidatableViewModel
    {
        private readonly CompositeDisposable _disposable;
        private readonly IUserService _userService;
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;
        private readonly ObservableCollection<AlbumVM> _allAlbums;
        private readonly ObservableCollection<UserVM> _allUsers;
        private readonly ReadOnlyObservableCollection<UserVM> _filteredUsers;
        private readonly ReadOnlyObservableCollection<AlbumVM> _filteredAlbums;
        private string? _albumSearch;
        private string? _userSearch;

        public AlbumsVM(
            IAlbumService albumService,
            IPhotoService photoService,
            IDialogService dialogService,
            IUserService userService)
        {
            ValidationContext = new ValidationContext();

            _albumService = NotNull(albumService, nameof(albumService));
            _photoService = NotNull(photoService, nameof(photoService));
            _userService = NotNull(userService, nameof(userService));
            _ = NotNull(dialogService, nameof(dialogService));

            _disposable = new CompositeDisposable();

            _allUsers = new ObservableCollection<UserVM>();

            var allUsersOCS = _allUsers.ToObservableChangeSet();

            var userSearch = this.WhenAnyValue(x => x.UserSearch)
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.TaskpoolScheduler)
                .Select(query => query?.Trim())
                .DistinctUntilChanged();

            var userFilter = userSearch
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(MakeUserFilter);

            var usersCache = allUsersOCS
                .Filter(userFilter)
                .AsObservableList();

            _ = usersCache.DisposeWith(_disposable);

            _ = usersCache
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _filteredUsers)
                .Subscribe()
                .DisposeWith(_disposable);

            _allAlbums = new ObservableCollection<AlbumVM>();

            var reactiveCommand = ReactiveCommand.CreateFromTask(GetAlbumsInternal);
            reactiveCommand.ThrownExceptions.Subscribe(e => dialogService.ShowDialog(e.Message));
            GetAlbums = reactiveCommand;

            var allAlbumsOCS = _allAlbums.ToObservableChangeSet();

            ClearAlbums = ReactiveCommand.Create(_allAlbums.Clear, allAlbumsOCS.IsNotEmpty());

            var albumSearchFilter = this.WhenAnyValue(x => x.AlbumSearch)
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.TaskpoolScheduler)
                .Select(query => query?.Trim())
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(MakeAlbumFilter);

            var userSearchFilter = userSearch
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(MakeUserSearchFilter);

            var userAlbumsOL = allAlbumsOCS
                .Filter(userSearchFilter)
                .AsObservableList();

            _ = userAlbumsOL.DisposeWith(_disposable);

            var albumsOL = userAlbumsOL
                .Connect()
                .Filter(albumSearchFilter)
                .AsObservableList();

            _ = albumsOL
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _filteredAlbums)
                .Subscribe()
                .DisposeWith(_disposable);

            Observable.StartAsync(GetUsersInternal);
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

        private async Task GetUsersInternal()
        {
            var users = await _userService.GetUsers();
            foreach (var user in users)
            {
                _allUsers.Add(new UserVM(user));
            }
        }

        public ValidationContext ValidationContext { get; }

        public ICommand GetAlbums { get; }

        public ICommand ClearAlbums { get; }

        public ReadOnlyObservableCollection<UserVM> Users => _filteredUsers;

        public ReadOnlyObservableCollection<AlbumVM> Albums => _filteredAlbums;

        public string? AlbumSearch
        {
            get => _albumSearch;
            set => this.RaiseAndSetIfChanged(ref _albumSearch, value);
        }

        public string? UserSearch
        {
            get => _userSearch;
            set => this.RaiseAndSetIfChanged(ref _userSearch, value);
        }

        private bool TryGetUser(string? search, out UserVM? user)
        {
            user = _allUsers.FirstOrDefault(u => !string.IsNullOrWhiteSpace(search) && u.Username.Contains(search, StringComparison.InvariantCultureIgnoreCase));

            return user != null;
        }
        
        private Func<AlbumVM, bool> MakeUserSearchFilter(string? filter)
        {
            return album => TryGetUser(filter, out UserVM? user) && album.UserId == user.Id;
        }

        private static Func<AlbumVM, bool> MakeAlbumFilter(string? filter)
        {
            return album => string.IsNullOrWhiteSpace(filter) || album.Title.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
        }

        private static Func<UserVM, bool> MakeUserFilter(string? filter)
        {
            return user => !string.IsNullOrWhiteSpace(filter) && user.Username.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
