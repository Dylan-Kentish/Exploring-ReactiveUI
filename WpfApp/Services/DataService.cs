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

    public async Task<IEnumerable<Album>> GetAlbums()
    {
        return await GetAsync<IEnumerable<Album>>(AlbumsPath) ?? Enumerable.Empty<Album>();
    }

    public async Task<IEnumerable<Photo>> GetAlbumPhotos(int albumId)
    {
        var photosUrl = PhotosPath.AddQuery(nameof(albumId), albumId.ToString());
        return await GetAsync<IEnumerable<Photo>>(photosUrl) ?? Enumerable.Empty<Photo>();
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await GetAsync<IEnumerable<User>>(UsersPath) ?? Enumerable.Empty<User>();
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

