using System;

namespace WpfApp.Services
{
    public interface INavigationService
    {
        bool CanGoBack { get; }

        bool CanGoForward { get; }

        string CurrentView { get; set; }

        void NavigateTo(string? tag);

        void GoBack();

        void GoForward();
    }
}
