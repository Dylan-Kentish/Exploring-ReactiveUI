using DynamicData;
using Prism.Regions;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Model;
using WpfApp.Services;
using static Microsoft.Requires;

namespace WpfApp.ViewModels;

public sealed class LoginVM : ReactiveValidationObject, IDisposable, IJournalAware
{
    private readonly INavigationService _navigationService;
    private readonly ActiveUser _activeUser;
    private readonly ReadOnlyObservableCollection<string> _usernames;
    private readonly IUserService _userService;
    private readonly CompositeDisposable _disposable;
    private readonly SourceList<User> _allUsers;
    private string? _username;

    public LoginVM(
        INavigationService navigationService,
        IUserService userService, 
        ActiveUser activeUser)
    {
        _navigationService = NotNull(navigationService, nameof(navigationService));
        _activeUser = NotNull(activeUser, nameof(activeUser));
        _userService = NotNull(userService, nameof(userService));
        _disposable = new CompositeDisposable();

        var usernameValid = this.WhenAnyValue(
            x => x.Username,
            UsernameIsValid);

        Login = ReactiveCommand.Create(LoginInternal, usernameValid);

        _allUsers = new SourceList<User>();

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
            .Synchronize()
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

    public ICommand Login { get; }

    public bool UsernameIsValid(string? queryUsername)
    {
        return !string.IsNullOrWhiteSpace(queryUsername) && _allUsers.Items.Any(user => user.Username == queryUsername);
    }

    public bool PersistInHistory() => false;

    private async Task GetUsersInternal()
    {
        var users = await _userService.GetUsers();
        foreach (var user in users)
        {
            _allUsers.Add(user);
        }
    }

    private static Func<string, bool> MakeUserFilter(string? queryUsername)
    {
        return username => !string.IsNullOrWhiteSpace(queryUsername) && (username?.Contains(queryUsername, StringComparison.InvariantCultureIgnoreCase) ?? false);
    }
        
    private void LoginInternal()
    {
        _activeUser.User = _allUsers.Items.First(user => user.Username == Username);
        _navigationService.NavigateTo(NavigationService.Account);
    }

    public void Dispose()
    {
        _disposable?.Dispose();
    }
}