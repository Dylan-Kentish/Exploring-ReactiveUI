using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using WpfApp.Model;
using WpfApp.Services;
using static Microsoft.Requires;

namespace WpfApp.ViewModels;

public sealed class AlbumVM : ReactiveObject, IDisposable
{
    private readonly CompositeDisposable _disposables;
    private readonly Album _model;
    private readonly INavigationService _navigationService;
    private readonly ObservableAsPropertyHelper<PhotoVM> _thumbnail;

    public AlbumVM(
        INavigationService navigationService,
        Album model)
    {
        _disposables = new CompositeDisposable();
        _navigationService = NotNull(navigationService, nameof(navigationService));
        _model = NotNull(model, nameof(model));

        Observable.StartAsync(model.GetPhotos)
            .Select(photos => photos.FirstOrDefault())
            .WhereNotNull()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Select(photo => new PhotoVM(photo))
            .ToProperty(this, x => x.Thumbnail, out _thumbnail)
            .DisposeWith(_disposables);

        OnClick = ReactiveCommand.Create(OnClickInternal);
    }

    public string Title => _model.Title;

    public PhotoVM Thumbnail => _thumbnail.Value;

    public ICommand OnClick { get; }

    private void OnClickInternal()
    {
        var parameters = new Dictionary<string, object>()
        {
            { nameof(Album), _model }
        };

        _navigationService.NavigateTo(NavigationService.Album, parameters);
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}