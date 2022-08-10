using System;
using System.Reactive.Disposables;
using ReactiveUI;
using static Microsoft.Requires;

namespace WpfApp.ViewModels
{
    public sealed class MainWindowVM : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();

        public MainWindowVM(ChangeThemeVM changeTheme, LoginVM loginVM)
        {
            ChangeThemeVM = NotNull(changeTheme, nameof(changeTheme));
            LoginVM = NotNull(loginVM, nameof(loginVM));

            _disposable.Add(loginVM);
        }


        public ChangeThemeVM ChangeThemeVM { get; }
        public LoginVM LoginVM { get; }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
