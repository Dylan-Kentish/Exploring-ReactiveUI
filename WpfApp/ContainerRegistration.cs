using Prism.Ioc;
using WpfApp.Services;
using WpfApp.ViewModels;

namespace WpfApp
{
    internal static class ContainerRegistration
    {
        public static void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var dataService = new DataService();

            containerRegistry.RegisterSingleton<IAlbumService>(() => dataService);
            containerRegistry.RegisterSingleton<IPhotoService>(() => dataService);
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.RegisterSingleton<MainWindowVM>();
            containerRegistry.RegisterSingleton<ChangeThemeVM>();
        }
    }
}
