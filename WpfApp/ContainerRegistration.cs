using Prism.Ioc;
using WpfApp.Service;
using WpfApp.ViewModels;

namespace WpfApp
{
    internal static class ContainerRegistration
    {
        public static void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAlbumService, AlbumService>();
            containerRegistry.RegisterSingleton<MainWindowVM>();
        }
    }
}
