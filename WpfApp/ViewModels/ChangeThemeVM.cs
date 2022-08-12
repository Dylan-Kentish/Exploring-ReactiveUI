using System.Reactive.Concurrency;
using System.Windows.Input;
using ModernWpf;
using ReactiveUI;

namespace WpfApp.ViewModels
{
    public class ChangeThemeVM
    {
        private readonly ThemeManager _themeManager;

        public ChangeThemeVM(ThemeManager themeManager)
        {
            _themeManager = themeManager;
            ChangeTheme = ReactiveCommand.Create(ChangeThemeInternal);
        }

        public ICommand ChangeTheme { get; }

        private void ChangeThemeInternal() =>
            RxApp.MainThreadScheduler.Schedule(() =>
                _themeManager.ApplicationTheme = 
                    _themeManager.ActualApplicationTheme == ApplicationTheme.Dark ?
                    ApplicationTheme.Light :
                    ApplicationTheme.Dark);
    }
}
