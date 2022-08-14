using System;

namespace WpfApp.Services
{
    public interface INavigationService
    {
        bool CanGoBack { get; }

        string CurrentView { get; set; }

        void NavigateTo(string? tag);

        void GoBack();
    }
}
