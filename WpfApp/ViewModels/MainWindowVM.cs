using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using ModernWpf.Controls;
using ReactiveUI;
using WpfApp.Model;
using static Microsoft.Requires;

namespace WpfApp.ViewModels
{
    public sealed class MainWindowVM : ReactiveObject, IDisposable
    {
        private Type? _currentPage;
        private CompositeDisposable _disposable;
        private readonly ObservableAsPropertyHelper<bool> _userLoggedIn;


        public MainWindowVM(ChangeThemeVM changeTheme, IObservable<User?> currentUser)
        {
            _disposable = new CompositeDisposable();

            ChangeThemeVM = NotNull(changeTheme, nameof(changeTheme));

            ViewSelected = ReactiveCommand.Create<NavigationViewItem>(ViewSelectedInternal);

            NavigateTo("HomePage");

            currentUser
                .Select(user => user != null)
                .ToProperty(this, x => x.UserLoggedIn, out _userLoggedIn)
                .DisposeWith(_disposable);
        }

        public ICommand ViewSelected { get; }

        public ChangeThemeVM ChangeThemeVM { get; }

        public Type? CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        public bool UserLoggedIn => _userLoggedIn.Value;

        private void ViewSelectedInternal(NavigationViewItem obj)
        {
            if (obj.Tag is string page)
            {
                NavigateTo(page);
            }
        }

        private void NavigateTo(string page)
        {
            // Really don't like this.
            // This could be improved by binding the NavigationView.MenuItems
            // to a collection maintained in this class.
            var typeString = "WpfApp.Views.Pages." + page;
            CurrentPage = Type.GetType(typeString);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
