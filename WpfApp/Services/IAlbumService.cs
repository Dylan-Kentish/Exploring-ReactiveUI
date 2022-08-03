using System.Collections.Generic;
using WpfApp.Model;

namespace WpfApp.Services;

public interface IAlbumService
{
    IEnumerable<Album> GetAlbums();
}