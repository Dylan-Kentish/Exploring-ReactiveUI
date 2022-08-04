using System;
using System.Reactive.Disposables;
using ReactiveUI;
using static Microsoft.Requires;

namespace WpfApp.ViewModels
{
    public sealed class MainWindowVM : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();

        public MainWindowVM(ChangeThemeVM changeTheme, AlbumsVM albums)
        {
            ChangeThemeVM = NotNull(changeTheme, nameof(changeTheme));
            AlbumsVM = NotNull(albums, nameof(albums));

            _disposable.Add(AlbumsVM);
        }


        public ChangeThemeVM ChangeThemeVM { get; }
        public AlbumsVM AlbumsVM { get; }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
