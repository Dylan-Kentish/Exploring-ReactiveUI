using System;
using System.Windows.Input;
using ModernWpf.Controls;
using ReactiveUI;
using static Microsoft.Requires;

namespace WpfApp.ViewModels
{
    public sealed class MainWindowVM : ReactiveObject
    {
        private Type? _currentPage;

        public MainWindowVM(ChangeThemeVM changeTheme)
        {
            ChangeThemeVM = NotNull(changeTheme, nameof(changeTheme));

            ViewSelected = ReactiveCommand.Create<NavigationViewItem>(ViewSelectedInternal);

            NavigateTo("HomePage");
        }

        public ICommand ViewSelected { get; }

        public ChangeThemeVM ChangeThemeVM { get; }

        public Type? CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

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
    }
}
