using System;
using ModernWpf;
using Prism.Ioc;
using ReactiveUI;
using WpfApp.Model;
using WpfApp.Services;
using WpfApp.ViewModels;

namespace WpfApp
{
    internal static class ContainerRegistration
    {
        public static void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterManySingleton<DataService>(
                typeof(IUserService),
                typeof(IAlbumService),
                typeof(IPhotoService));

            var activeUser = new ActiveUser();

            var observableUser = activeUser.WhenAnyValue(au => au.User);

            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.RegisterSingleton<INavigationService, NavigationService>();
            containerRegistry.RegisterSingleton<ThemeManager>(() => ThemeManager.Current);

            containerRegistry.RegisterSingleton<ActiveUser>(() => activeUser);
            containerRegistry.RegisterSingleton<IObservable<User>>(() => observableUser);

            containerRegistry.RegisterSingleton<ChangeThemeVM>();
            containerRegistry.RegisterSingleton<MainWindowVM>();
        }
    }
}
