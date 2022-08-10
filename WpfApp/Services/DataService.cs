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

public class DataService : IAlbumService, IPhotoService, IUserService
{
    private const string UriString = "https://jsonplaceholder.typicode.com/";
    private const string UsersPath = UriString + "users";
    private const string AlbumsPath = UriString + "albums";
    private const string PhotosPath = UriString + "photos";

    private readonly HttpClient _client = new();

    public DataService()
    {
        _client.BaseAddress = new Uri(UriString);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<IEnumerable<Album>> GetUserAlbums(User user)
    {
        var albumsUrl = AlbumsPath.AddQuery("userId", user.Id.ToString());

        var albums = await GetAsync<IEnumerable<API.Album>>(albumsUrl);

        if (albums is null)
        {
            return Enumerable.Empty<Album>();
        }

        return albums.Select(api => new Album(this, api, user));
    }

    public async Task<IEnumerable<Photo>> GetAlbumPhotos(Album album)
    {
        var photosUrl = PhotosPath.AddQuery("albumId", album.Id.ToString());

        var photos = await GetAsync<IEnumerable<API.Photo>>(photosUrl);

        if (photos is null)
        {
            return Enumerable.Empty<Photo>();
        }

        return photos.Select(api => new Photo(api, album));
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        var users = await GetAsync<IEnumerable<API.User>>(UsersPath);

        if (users is null)
        {
            return Enumerable.Empty<User>();
        }

        return users.Select(api => new User(this, api));
    }

    private async Task<T?> GetAsync<T>(string url)
    {
        HttpResponseMessage response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<T>();
        }

        return await Task.FromResult<T?>(default);
    }
}

