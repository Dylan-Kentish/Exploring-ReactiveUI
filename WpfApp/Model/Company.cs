namespace WpfApp.Model;

public class Company
{
    public Company(API.Company company)
    {
        Name = company.Name;
        CatchPhrase = company.CatchPhrase;
        BS = company.BS;
    }
    
    public string Name { get; }
    public string CatchPhrase { get; }
    public string BS { get; }
}