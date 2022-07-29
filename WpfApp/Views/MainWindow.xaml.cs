using System.Reflection;
using System.Windows;
using WpfApp.ViewModels;
using static Microsoft.Requires;

namespace WpfApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowVM viewModel)
        {
            DataContext = NotNull(viewModel, nameof(viewModel));
            InitializeComponent();

            Title = "Wpf App";

            var version = Assembly.GetEntryAssembly()?.GetName().Version;

            if(version != null)
            {
                Title += $" v{version}";
            }
        }
    }
}
