using System.Text.Json.Serialization;

namespace WpfApp.API;

public struct Address
{
    [JsonConstructor]
    public Address(string street, string suite, string city, string zipCode, Geo geo)
    {
        Street = street;
        Suite = suite;
        City = city;
        ZipCode = zipCode;
        Geo = geo;
    }
    
    public string Street { get; }
    public string Suite { get; }
    public string City { get; }
    public string ZipCode { get; }
    public Geo Geo { get; }
}