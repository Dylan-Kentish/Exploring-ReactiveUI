using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Extensions
{
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
}
