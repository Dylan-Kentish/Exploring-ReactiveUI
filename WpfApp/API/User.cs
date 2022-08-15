using System.Text.Json.Serialization;

namespace WpfApp.API;

public struct User
{
    [JsonConstructor]
    public User(int id, string name, string username)
    {
        Id = id;
        Name = name;
        Username = username;
    }

    public int Id { get; }
    public string Name { get; }
    public string Username { get; }
}