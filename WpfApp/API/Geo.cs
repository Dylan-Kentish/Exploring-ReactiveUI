using System.Text.Json.Serialization;

namespace WpfApp.API;

public struct Geo
{
    [JsonConstructor]
    public Geo(double lat, double lng)
    {
        Lat = lat;
        Lng = lng;
    }

    public double Lat { get; }
    public double Lng { get; }
}