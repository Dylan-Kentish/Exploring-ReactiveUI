using WpfApp.Model;

namespace WpfApp.ViewModels;

public class UserVM
{
    private readonly User _model;

    public UserVM(User model)
    {
        _model = model;
    }

    public string Username => _model.Username;
}