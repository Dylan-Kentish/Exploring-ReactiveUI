﻿using System;
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
        private readonly INavigationService _navigationService;
        private readonly CompositeDisposable _disposable;
        private readonly ObservableAsPropertyHelper<bool> _userLoggedIn;
        private string? _selectedTag;

        public MainWindowVM(
            ChangeThemeVM changeTheme, 
            IObservable<User?> currentUser,
            INavigationService navigationService)
        {
            _navigationService = navigationService;
            _disposable = new CompositeDisposable();

            ChangeThemeVM = NotNull(changeTheme, nameof(changeTheme));

            var cgbObservable = _navigationService
                .WhenAnyValue(x => x.CanGoBack);

            BackRequested = ReactiveCommand.Create(_navigationService.GoBack, cgbObservable);

            currentUser
                .Select(user => user != null)
                .ToProperty(this, x => x.UserLoggedIn, out _userLoggedIn)
                .DisposeWith(_disposable);

            this.WhenAnyValue(x => x.SelectedTag)
                .Subscribe(_navigationService.NavigateTo)
                .DisposeWith(_disposable);

            _navigationService.WhenAnyValue(x => x.CurrentView)
                .Subscribe(tag => SelectedTag = tag)
                .DisposeWith(_disposable);
        }

        public ICommand BackRequested { get; }

        public ChangeThemeVM ChangeThemeVM { get; }

        public bool UserLoggedIn => _userLoggedIn.Value;

        public string? SelectedTag
        {
            get => _selectedTag;
            set => this.RaiseAndSetIfChanged(ref _selectedTag, value);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
