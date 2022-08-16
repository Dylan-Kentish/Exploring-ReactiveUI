namespace WpfApp.Model;

public class Address
{
    public Address(API.Address address)
    {
        Street = address.Street;
        Suite = address.Suite;
        City = address.City;
        ZipCode = address.ZipCode;
        Geo = new Geo(address.Geo);
    }
    
    public string Street { get; }
    public string Suite { get; }
    public string City { get; }
    public string ZipCode { get; }
    public Geo Geo { get; }
}