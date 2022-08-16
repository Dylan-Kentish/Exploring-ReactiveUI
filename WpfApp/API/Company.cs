using System.Text.Json.Serialization;

namespace WpfApp.API;

public struct Company
{
    [JsonConstructor]
    public Company(string name, string catchPhrase, string bs)
    {
        Name = name;
        CatchPhrase = catchPhrase;
        BS = bs;
    }
    
    public string Name { get; }
    public string CatchPhrase { get; }
    public string BS { get; }
}