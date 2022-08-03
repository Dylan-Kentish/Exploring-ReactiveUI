using System.Collections.Generic;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Services;

public interface IAlbumService
{
    Task<IEnumerable<Album>> GetAlbums();
}