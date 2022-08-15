using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
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
        private bool _canGoForward;
        private IRegion? _mainRegion;
        private IRegionNavigationJournal? _journal;
        private User? _activeUser;
        private string? _currentView;
        private bool _navigating;

        public NavigationService(
            IRegionManager regionManager, 
            IObservable<User?> activeUser)
        {
            _regionManager = regionManager;
            _disposable = new CompositeDisposable();

            activeUser
                .Subscribe(ActiveUserChanged)
                .DisposeWith(_disposable);
        }

        public bool CanGoBack
        {
            get => _canGoBack;
            private set => this.RaiseAndSetIfChanged(ref _canGoBack, value);
        }

        public bool CanGoForward
        {
            get => _canGoForward;
            private set => this.RaiseAndSetIfChanged(ref _canGoForward, value);
        }

        public string? CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        public void NavigateTo(string? tag, Dictionary<string, object>? parameters = null)
        {
            if (_navigating || tag is null)
            {
                return;
            }

            if (_mainRegion is null &&
                !GetMainRegion())
            {
                if (parameters is null)
                {
                    _regionManager.RegisterViewWithRegion(MainRegion, tag);
                }
                return;
            }

            var navigationParameters = new NavigationParameters();
            if (parameters is not null)
            {
                foreach (var item in parameters)
                {
                    navigationParameters.Add(item.Key, item.Value);
                }
            }

            
            _navigating = true;

            if (_activeUser is null && tag is Account or AccountDetails)
            {
                _mainRegion.RequestNavigate(Login, OnNavigation, navigationParameters);
            }
            else
            {
                _mainRegion.RequestNavigate(tag, OnNavigation, navigationParameters);
            }
        }

        public void GoBack()
        {
            _navigating = true;
            _journal?.GoBack();
            _navigating = false;
            UpdateCanGoBack();
            UpdateCanGoForward();
        }

        public void GoForward()
        {
            _navigating = true;
            _journal?.GoForward();
            _navigating = false;
            UpdateCanGoBack();
            UpdateCanGoForward();
        }

        private void ActiveUserChanged(User? user)
        {
            _activeUser = user;
        }

        private bool GetMainRegion()
        {
            if (_mainRegion is null && _regionManager.Regions.ContainsRegionWithName(MainRegion))
            {
                _mainRegion = _regionManager.Regions[MainRegion];
                _mainRegion.NavigationService.Navigated += NavigationService_Navigated;

                _journal = _mainRegion.NavigationService.Journal;
            }

            return _mainRegion != null;
        }

        private void NavigationService_Navigated(object? sender, RegionNavigationEventArgs e)
        {
            UpdateCurrentView(e.NavigationContext);
        }

        private void OnNavigation(NavigationResult e)
        {
            UpdateCanGoBack();
            UpdateCanGoForward();
            UpdateCurrentView(e.Context);
            _navigating = false;
        }

        private void UpdateCanGoBack()
        {
            CanGoBack = _journal?.CanGoBack ?? false;
        }

        private void UpdateCanGoForward()
        {
            CanGoForward = _journal?.CanGoForward ?? false;
        }

        private void UpdateCurrentView(NavigationContext e)
        {
            CurrentView = e.Uri.OriginalString;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
