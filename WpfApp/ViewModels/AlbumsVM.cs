using ReactiveUI;
using ReactiveUI.Validation.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using WpfApp.Model;
using DynamicData;
using Prism.Regions;
using WpfApp.Services;
using static Microsoft.Requires;

namespace WpfApp.ViewModels;

public sealed class AlbumsVM : ReactiveValidationObject, IDisposable, INavigationAware
{
    private readonly CompositeDisposable _disposables;
    private readonly INavigationService _navigationService;
    private ReadOnlyObservableCollection<AlbumVM>? _albums;
    private string? _albumSearch;
    private User? _user;

    public AlbumsVM(
        INavigationService navigationService)
    {
        _navigationService = NotNull(navigationService, nameof(navigationService));
        _disposables = new CompositeDisposable();
    }

    public ReadOnlyObservableCollection<AlbumVM>? Albums => _albums;

    public string? AlbumSearch
    {
        get => _albumSearch;
        set => this.RaiseAndSetIfChanged(ref _albumSearch, value);
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (_user is not null || navigationContext.Parameters["User"] is not User user)
        {
            return;
        }

        _user = user;
        
        var cache = new SourceCache<Album, int>(a => a.Id);
        cache.DisposeWith(_disposables);

        var albumSearchFilter = this.WhenAnyValue(x => x.AlbumSearch)
            .Throttle(TimeSpan.FromSeconds(0.5), RxApp.TaskpoolScheduler)
            .Select(query => query?.Trim())
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Select(MakeAlbumFilter);

        cache
            .Connect()
            .Filter(albumSearchFilter)
            .Transform(x => new AlbumVM(_navigationService, x))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _albums)
            .Subscribe()
            .DisposeWith(_disposables);

        Observable.StartAsync(user.GetAlbums, RxApp.TaskpoolScheduler)
            .Subscribe(albums =>
            {
                cache.Edit(innerCache =>
                {
                    innerCache.Clear();
                    innerCache.AddOrUpdate(albums);
                });
            }).DisposeWith(_disposables);
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return navigationContext.Parameters["User"] is User user &&
               _user?.Id == user.Id;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }

    private static Func<Album, bool> MakeAlbumFilter(string? filter)
    {
        return album => string.IsNullOrWhiteSpace(filter) || album.Title.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}