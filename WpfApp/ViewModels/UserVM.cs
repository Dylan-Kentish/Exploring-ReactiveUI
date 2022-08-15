using WpfApp.Model;
using static Microsoft.Requires;

namespace WpfApp.ViewModels;

public class UserVM
{
    private readonly User _model;

    public UserVM(User model)
    {
        _model = NotNull(model, nameof(model)) ;
    }

    public int Id => _model.Id;

    public string Username => _model.Username;

    public override string ToString() => Username;
}