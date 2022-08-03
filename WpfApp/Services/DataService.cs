using System.Collections.Generic;
using WpfApp.Model;

namespace WpfApp.Services;

public class DataService : IAlbumService
{
    public IEnumerable<Album> GetAlbums()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new Album(i, $"{i}: Lorem Ipsum");
        }
    }
}

