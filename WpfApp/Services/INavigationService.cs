using System;

namespace WpfApp.Services
{
    public interface INavigationService
    {
        void NavigateTo(string tag);

        Type? CurrentPage { get; }
    }
}
