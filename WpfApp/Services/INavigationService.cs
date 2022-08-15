using System.Collections.Generic;

namespace WpfApp.Services
{
    public interface INavigationService
    {
        bool CanGoBack { get; }

        bool CanGoForward { get; }

        string? CurrentView { get; set; }

        void NavigateTo(string? tag, Dictionary<string, object>? parameters = null);

        void GoBack();

        void GoForward();
    }
}
