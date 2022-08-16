using DynamicData;
using Prism.Regions;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using WpfApp.Model;

namespace WpfApp.ViewModels;

internal class PhotosVM : ReactiveObject, IDisposable, INavigationAware
{
    private const int Size = 6;
    private readonly CompositeDisposable _disposables;
    private IVirtualRequest _virtualRequest;
    private Album? _model;
    private readonly ReadOnlyObservableCollection<PhotoVM>? _photos;
    private string? _photoSearch;
    private readonly SourceCache<Photo, int> _cache;

    public PhotosVM()
    {
        _disposables = new CompositeDisposable();
        _virtualRequest = new VirtualRequest(0, Size);
        _cache = new SourceCache<Photo, int>(photo => photo.Id);
        _cache.DisposeWith(_disposables);

        var photosSearchFilter = this.WhenAnyValue(x => x.PhotoSearch)
            .Throttle(TimeSpan.FromSeconds(0.5), RxApp.TaskpoolScheduler)
            .Select(query => query?.Trim())
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Select(MakePhotoFilter);

        var virtualRequests = this.WhenAnyValue(x => x.VirtualRequest);

        var orderedCache = _cache
            .Connect()
            .Filter(photosSearchFilter)
            .Transform(x => new PhotoVM(x))
            .SortBy(photo => photo.Title);

        var observableOrderedCache = orderedCache.AsObservableCache();

        observableOrderedCache.DisposeWith(_disposables);

        orderedCache
            .Virtualise(virtualRequests)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _photos)
            .Subscribe()
            .DisposeWith(_disposables);

        var canNext = virtualRequests
            .ObserveOn(RxApp.MainThreadScheduler)
            .CombineLatest(observableOrderedCache.CountChanged,
            (r, c) =>
            {
                var endIndex = r.StartIndex + r.Size;
                var can = c - endIndex - 1 > 0;

                if (r.StartIndex > c - 1)
                {
                    VirtualRequest = new VirtualRequest(c / Size, Size);
                }

                return can;
            });

        var canPrevious = virtualRequests
            .ObserveOn(RxApp.MainThreadScheduler)
            .Select(request => request.StartIndex != 0);

        Previous = ReactiveCommand.Create(PreviousInternal, canPrevious);
        Next = ReactiveCommand.Create(NextInternal, canNext);
    }

    public ReadOnlyObservableCollection<PhotoVM>? Photos => _photos;

    public string? PhotoSearch
    {
        get => _photoSearch;
        set => this.RaiseAndSetIfChanged(ref _photoSearch, value);
    }

    public ICommand Previous { get; }

    public ICommand Next { get; }

    private IVirtualRequest VirtualRequest
    {
        get => _virtualRequest;
        set => this.RaiseAndSetIfChanged(ref _virtualRequest, value);
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

        Observable.StartAsync(_model.GetPhotos, RxApp.TaskpoolScheduler)
            .Subscribe(photos =>
            {
                _cache.Edit(innerCache =>
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

    private void PreviousInternal()
    {
        VirtualRequest = new VirtualRequest(VirtualRequest.StartIndex - Size, Size);
    }

    private void NextInternal()
    {
        VirtualRequest = new VirtualRequest(VirtualRequest.StartIndex + Size, Size);
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}