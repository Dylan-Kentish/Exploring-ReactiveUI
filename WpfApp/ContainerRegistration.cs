using Prism.Ioc;
using WpfApp.ViewModels;

namespace WpfApp
{
    internal static class ContainerRegistration
    {
        public static void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<MainWindowVM>();
        }
    }
}
