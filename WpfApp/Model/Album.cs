using System.Collections.Generic;
using System.Threading.Tasks;
using WpfApp.Services;

namespace WpfApp.Model
{
    public class Album
    {
        private readonly IPhotoService _photoService;

        public Album(IPhotoService photoService, API.Album album, User user)
        {
            User = user;
            _photoService = photoService;
            Id = album.Id;
            Title = album.Title;
        }

        public User User { get; }
        public int Id { get; }
        public string Title { get; }
        public IEnumerable<Photo>? Photos { get; private set; }

        public async Task GetPhotos()
        {
            Photos = await _photoService.GetAlbumPhotos(this);
        }
    }
}
