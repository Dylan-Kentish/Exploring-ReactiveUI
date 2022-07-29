using System.Collections.Generic;
using WpfApp.Model;

namespace WpfApp.Service;

public interface IAlbumService
{
    IEnumerable<Album> GetAlbums();
}