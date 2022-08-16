namespace WpfApp.Model;

public class Geo
{
    public Geo(API.Geo geo)
    {
        Lat = geo.Lat;
        Lng = geo.Lng;
    }

    public double Lat { get; }
    public double Lng { get; }
}