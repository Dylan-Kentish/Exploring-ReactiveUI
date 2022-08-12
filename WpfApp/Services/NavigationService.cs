using System;
using ReactiveUI;
using WpfApp.Model;

namespace WpfApp.Services
{
    public class NavigationService : ReactiveObject, INavigationService
    {
        private const string PagesNamespace = "WpfApp.Views.Pages.";
        private const string Page = nameof(Page);
        private const string Login = nameof(Login);

        public const string Home = nameof(Home);
        public const string Account = nameof(Account);
        public const string AccountDetails = nameof(AccountDetails);
        public const string Albums = nameof(Albums);
        public const string Album = nameof(Album);
        public const string Posts = nameof(Posts);
        public const string Post = nameof(Post);


        private User? _activeUser;
        private Type? _currentPage;

        public NavigationService(IObservable<User?> activeUser)
        {
            activeUser.Subscribe(ActiveUserChanged);

            NavigateTo(Home);
        }

        public void NavigateTo(string tag)
        {
            var typeString = PagesNamespace;

            if (tag == Account && _activeUser != null)
            {
                typeString += Login;
            }
            else
            {
                typeString += tag;
            }

            CurrentPage = Type.GetType(typeString + Page);
        }

        public Type? CurrentPage
        {
            get => _currentPage;
            private set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        private void ActiveUserChanged(User? user)
        {
            _activeUser = user;
        }
    }
}
