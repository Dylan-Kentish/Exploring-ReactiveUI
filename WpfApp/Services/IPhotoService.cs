using System.Collections.Generic;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Services;

public interface IPhotoService
{
    Task<IEnumerable<Photo>> GetAlbumPhotos(int albumId);
}