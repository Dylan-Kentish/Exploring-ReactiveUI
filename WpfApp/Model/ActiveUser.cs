using ReactiveUI;

namespace WpfApp.Model;

public class ActiveUser : ReactiveObject
{
    private User? _user;

    public User? User
    {
        get => _user;
        set => this.RaiseAndSetIfChanged(ref _user, value);
    }
}