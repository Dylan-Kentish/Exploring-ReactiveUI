using System.Text.Json.Serialization;

namespace WpfApp.API;

public struct User
{
    [JsonConstructor]
    public User(int id,
        string name,
        string username,
        string email,
        Address address,
        string phone,
        string website,
        Company company)
    {
        Id = id;
        Name = name;
        Username = username;
        Email = email;
        Address = address;
        Phone = phone;
        Website = website;
        Company = company;
    }

    public int Id { get; }
    public string Name { get; }
    public string Username { get; }
    public string Email { get; }
    public Address Address { get; }
    public string Phone { get; }
    public string Website { get; }
    public Company Company { get; }
}