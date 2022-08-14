using System;
using System.Reactive.Disposables;
using Prism.Ioc;
using Prism.Regions;
using ReactiveUI;
using WpfApp.Model;

namespace WpfApp.Services
{
    public sealed class NavigationService : ReactiveObject, INavigationService, IDisposable
    {
        public const string MainRegion = nameof(MainRegion);

        public const string Home = nameof(Home);
        public const string Login = nameof(Login);
        public const string Account = nameof(Account);
        public const string AccountDetails = nameof(AccountDetails);
        public const string Albums = nameof(Albums);
        public const string Album = nameof(Album);
        public const string Posts = nameof(Posts);
        public const string Post = nameof(Post);

        private readonly IRegionManager _regionManager;
        private readonly CompositeDisposable _disposable;
        private bool _canGoBack;
        private IRegion? _mainRegion;
        private IRegionNavigationJournal? _journal;
        private User? _activeUser;

        public NavigationService(
            IRegionManager regionManager, 
            IObservable<User?> activeUser)
        {
            _regionManager = regionManager;
            _disposable = new CompositeDisposable();

            activeUser
                .Subscribe(ActiveUserChanged)
                .DisposeWith(_disposable);

            NavigateTo(Home);
        }

        public bool CanGoBack
        {
            get => _canGoBack;
            private set => this.RaiseAndSetIfChanged(ref _canGoBack, value);
        }

        public void NavigateTo(string? tag)
        {
            if (_mainRegion is null &&
                !GetMainRegion())
            {
                return;
            }

            if (_activeUser is null && (tag == Account || tag == AccountDetails))
            {
                _mainRegion.RequestNavigate(Login);
            }
            else
            {
                _mainRegion.RequestNavigate(tag, _ => UpdateCanGoBack());
            }
        }

        public void GoBack()
        {
            _journal?.GoBack();
            UpdateCanGoBack();
        }

        private void ActiveUserChanged(User? user)
        {
            _activeUser = user;
        }

        private bool GetMainRegion()
        {
            if (_mainRegion is null)
            {
                if (_regionManager.Regions.ContainsRegionWithName(MainRegion))
                {
                    _mainRegion = _regionManager.Regions[MainRegion];

                    _journal = _mainRegion.NavigationService.Journal;
                }
            }

            return _mainRegion != null;
        }

        private void UpdateCanGoBack()
        {
            CanGoBack = _journal?.CanGoBack ?? false;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
