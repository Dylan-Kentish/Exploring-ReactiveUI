using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WpfApp.Services;
using static Microsoft.Requires;

namespace WpfApp.ViewModels
{
    public sealed class LoginVM : ReactiveValidationObject, IDisposable
    {
        private readonly ReadOnlyObservableCollection<string> _usernames;
        private readonly IUserService _userService;
        private readonly CompositeDisposable _disposable;
        private readonly SourceList<UserVM> _allUsers;
        private string? _username;

        public LoginVM(IUserService userService)
        {
            _userService = NotNull(userService, nameof(userService));
            _disposable = new CompositeDisposable();

            _allUsers = new SourceList<UserVM>();

            var userFilter = this.WhenAnyValue(x => x.Username)
                .Select(query => query?.Trim())
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(MakeUserFilter);

            var filteredUsernames = _allUsers
                .Connect()
                .Transform(user => user.Username)
                .Filter(userFilter)
                .AsObservableList();

            filteredUsernames.DisposeWith(_disposable);

            filteredUsernames
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _usernames)
                .Subscribe()
                .DisposeWith(_disposable);

            this.ValidationRule(
                vm => vm.Username,
                UsernameIsValid,
                "Username invalid.");

            Observable.Start(GetUsersInternal); // better alternatives?
        }

        public ReadOnlyObservableCollection<string> Usernames => _usernames;

        public string? Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        public bool UsernameIsValid(string? queryUsername)
        {
            return !string.IsNullOrWhiteSpace(queryUsername) && _allUsers.Items.Any(user => user.Username == queryUsername);
        }

        private async Task GetUsersInternal()
        {
            var users = await _userService.GetUsers();
            foreach (var user in users)
            {
                _allUsers.Add(new UserVM(user));
            }
        }

        private static Func<string, bool> MakeUserFilter(string? queryUsername)
        {
            return username => !string.IsNullOrWhiteSpace(queryUsername) && username.Contains(queryUsername, StringComparison.InvariantCultureIgnoreCase);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
