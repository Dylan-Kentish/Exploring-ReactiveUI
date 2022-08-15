using System;
using ModernWpf;
using Prism.Ioc;
using Prism.Regions;
using ReactiveUI;
using WpfApp.Model;
using WpfApp.Services;
using WpfApp.ViewModels;
using WpfApp.Views.Pages;

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

            containerRegistry.RegisterSingleton<IRegionManager, RegionManager>();
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.RegisterSingleton<INavigationService, NavigationService>();
            containerRegistry.RegisterSingleton<ThemeManager>(() => ThemeManager.Current);

            containerRegistry.RegisterSingleton<ActiveUser>(() => activeUser);
            containerRegistry.RegisterSingleton<IObservable<User?>>(() => observableUser);

            containerRegistry.RegisterSingleton<ChangeThemeVM>();
            containerRegistry.RegisterSingleton<MainWindowVM>();

            containerRegistry.RegisterForNavigation<HomePage>(NavigationService.Home);
            containerRegistry.RegisterForNavigation<LoginPage, LoginVM>(NavigationService.Login);
            containerRegistry.RegisterForNavigation<AccountPage, AccountVM>(NavigationService.Account);
            containerRegistry.RegisterForNavigation<AlbumsPage, AlbumsVM>(NavigationService.Albums);
            containerRegistry.RegisterForNavigation<AlbumPage, PhotosVM>(NavigationService.Album);
        }
    }
}
