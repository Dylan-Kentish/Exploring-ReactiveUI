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

    public string Name => _model.Name;

    public string Username => _model.Username;

    public string Email => _model.Email;

    public Address Address => _model.Address;

    public string Phone => _model.Phone;

    public string Website => _model.Website;

    public Company Company => _model.Company;

    public override string ToString() => Username;
}