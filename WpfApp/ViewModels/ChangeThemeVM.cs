using System.Reactive.Concurrency;
using System.Windows.Input;
using ModernWpf;
using ReactiveUI;

namespace WpfApp.ViewModels
{
    public class ChangeThemeVM
    {
        public ChangeThemeVM()
        {
            ChangeTheme = ReactiveCommand.Create(ChangeThemeInternal);
        }

        public ICommand ChangeTheme { get; }

        private static void ChangeThemeInternal()
        {
            RxApp.MainThreadScheduler.Schedule(() =>
            {
                if (ThemeManager.Current.ActualApplicationTheme == ApplicationTheme.Dark)
                {
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
                }
                else
                {
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                }
            });
        }
    }
}
