using System.Text.Json.Serialization;

namespace WpfApp.API;

public struct Album
{
    [JsonConstructor]
    public Album(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public int Id { get; }
    public string Title { get; }
}