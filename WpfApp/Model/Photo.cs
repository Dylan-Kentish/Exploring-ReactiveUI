namespace WpfApp.Model
{
    public class Photo
    {
        public Photo(API.Photo photo, Album album)
        {
            Album = album;
            Id = photo.Id;
            Title = photo.Title;
            Url = photo.Url;
            ThumbnailUrl = photo.ThumbnailUrl;
        }

        public Album Album { get; }
        public int Id { get; }
        public string Title { get; }
        public string Url { get; }
        public string ThumbnailUrl { get; }

    }
}
