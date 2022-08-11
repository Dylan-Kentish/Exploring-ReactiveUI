using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WpfApp.Extensions;
using WpfApp.Model;


namespace WpfApp.Services;

public sealed class DataService : IAlbumService, IPhotoService, IUserService
{
    private static readonly SocketsHttpHandler HttpClientHandler = new()
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(1),
    };

    private const string UriString = "https://jsonplaceholder.typicode.com/";
    private const string UsersPath = UriString + "users";
    private const string AlbumsPath = UriString + "albums";
    private const string PhotosPath = UriString + "photos";

    public async Task<IEnumerable<Album>> GetUserAlbums(User user)
    {
        var albumsUrl = AlbumsPath.AddQuery("userId", user.Id.ToString());

        var albums = await GetAsync<API.Album>(albumsUrl);

        return albums.Select(api => new Album(this, api, user));
    }

    public async Task<IEnumerable<Photo>> GetAlbumPhotos(Album album)
    {
        var photosUrl = PhotosPath.AddQuery("albumId", album.Id.ToString());

        var photos = await GetAsync<API.Photo>(photosUrl);

        return photos.Select(api => new Photo(api, album));
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        var users = await GetAsync<API.User>(UsersPath);

        return users.Select(api => new User(this, api));
    }

    private static async Task<IEnumerable<T>> GetAsync<T>(string url)
    {
        IEnumerable<T>? result = default;

        using (var client = new HttpClient(HttpClientHandler, disposeHandler: false))
        {
            client.BaseAddress = new Uri(UriString);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadFromJsonAsync<IEnumerable<T>>() ;
                }
            }
        }

        return result ?? Enumerable.Empty<T>();
    }
}

