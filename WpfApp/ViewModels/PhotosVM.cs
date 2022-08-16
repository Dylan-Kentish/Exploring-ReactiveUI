using DynamicData;
using Prism.Regions;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using WpfApp.Model;

namespace WpfApp.ViewModels;

internal class PhotosVM : ReactiveObject, IDisposable, INavigationAware
{
    private readonly CompositeDisposable _disposables;
    private Album? _model;
    private ReadOnlyObservableCollection<PhotoVM>? _photos;
    private string? _photoSearch;

    public PhotosVM()
    {
        _disposables = new CompositeDisposable();
    }

    public ReadOnlyObservableCollection<PhotoVM>? Photos => _photos;

    public string? PhotoSearch
    {
        get => _photoSearch;
        set => this.RaiseAndSetIfChanged(ref _photoSearch, value);
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return navigationContext.Parameters["Album"] is Album model &&
               _model?.Id == model.Id;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (_model is not null || navigationContext.Parameters["Album"] is not Album model)
        {
            return;
        }

        _model = model;

        var cache = new SourceCache<Photo, int>(photo => photo.Id);
        cache.DisposeWith(_disposables);

        var photosSearchFilter = this.WhenAnyValue(x => x.PhotoSearch)
            .Throttle(TimeSpan.FromSeconds(0.5), RxApp.TaskpoolScheduler)
            .Select(query => query?.Trim())
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Select(MakePhotoFilter);

        cache
            .Connect()
            .Filter(photosSearchFilter)
            .Transform(x => new PhotoVM(x))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _photos)
            .Subscribe()
            .DisposeWith(_disposables);
        
        Observable.StartAsync(_model.GetPhotos, RxApp.TaskpoolScheduler)
            .Subscribe(photos =>
            {
                cache.Edit(innerCache =>
                {
                    innerCache.Clear();
                    innerCache.AddOrUpdate(photos);
                });
            });
    }

    private static Func<Photo, bool> MakePhotoFilter(string? filter)
    {
        return album => string.IsNullOrWhiteSpace(filter) || album.Title.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}