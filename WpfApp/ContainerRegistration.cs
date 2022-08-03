using Prism.Ioc;
using WpfApp.Services;
using WpfApp.ViewModels;

namespace WpfApp
{
    internal static class ContainerRegistration
    {
        public static void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAlbumService, DataService>();
            containerRegistry.RegisterSingleton<MainWindowVM>();
        }
    }
}
