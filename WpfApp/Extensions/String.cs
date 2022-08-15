namespace WpfApp.Extensions;

internal static class String
{
    internal static string AddQuery(this string url, string queryProperty, string queryValue)
    {
        if (url.Contains('?'))
        {
            return url + $"&{queryProperty}={queryValue}";
        }
        return url + $"?{queryProperty}={queryValue}";
    }
}