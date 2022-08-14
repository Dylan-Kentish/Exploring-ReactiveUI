using System;

namespace WpfApp.Services
{
    public interface INavigationService
    {
        bool CanGoBack { get; }

        void NavigateTo(string? tag);

        void GoBack();
    }
}
