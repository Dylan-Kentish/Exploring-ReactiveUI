using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using WpfApp.Model;
using WpfApp.Services;
using static Microsoft.Requires;

namespace WpfApp.ViewModels
{
    public sealed class MainWindowVM : ReactiveObject, IDisposable
    {
        private readonly IObservable<User?> _currentUser;
        private readonly INavigationService _navigationService;
        private readonly CompositeDisposable _disposable;
        private readonly ObservableAsPropertyHelper<bool> _userLoggedIn;
        private string? _selectedTag;

        public MainWindowVM(
            ChangeThemeVM changeTheme, 
            IObservable<User?> currentUser,
            INavigationService navigationService)
        {
            ChangeThemeVM = NotNull(changeTheme, nameof(changeTheme));
            _currentUser = NotNull(currentUser, nameof(currentUser));
            _navigationService = NotNull(navigationService, nameof(navigationService));
            _disposable = new CompositeDisposable();

            var cgbObservable = navigationService
                .WhenAnyValue(x => x.CanGoBack);

            var cgfObservable = navigationService
                .WhenAnyValue(x => x.CanGoForward);

            BackRequested = ReactiveCommand.Create(navigationService.GoBack, cgbObservable);

            ForwardRequested = ReactiveCommand.Create(navigationService.GoForward, cgfObservable);

            currentUser
                .Select(user => user is not null)
                .ToProperty(this, x => x.UserLoggedIn, out _userLoggedIn)
                .DisposeWith(_disposable);

            this.WhenAnyValue(x => x.SelectedTag)
                .Subscribe(OnSelectedTagChanged)
                .DisposeWith(_disposable);

            navigationService.WhenAnyValue(x => x.CurrentView)
                .Subscribe(tag => SelectedTag = tag)
                .DisposeWith(_disposable);

            SelectedTag = NavigationService.Home;
        }

        public ICommand BackRequested { get; }

        public ICommand ForwardRequested { get; }

        public ChangeThemeVM ChangeThemeVM { get; }

        public bool UserLoggedIn => _userLoggedIn.Value;

        public string? SelectedTag
        {
            get => _selectedTag;
            set => this.RaiseAndSetIfChanged(ref _selectedTag, value);
        }

        private void OnSelectedTagChanged(string? tag)
        {
            Dictionary<string, object> parameters = new();

            switch (tag)
            {
                case NavigationService.Albums:
                    var user = _currentUser.WhereNotNull().Latest().First();
                    parameters.Add(nameof(User), user);
                    break;
            }

            _navigationService.NavigateTo(tag, parameters);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
