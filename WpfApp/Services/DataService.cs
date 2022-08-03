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

public class DataService : IAlbumService, IPhotoService
{
    private const string UriString = "https://jsonplaceholder.typicode.com/";
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
        IEnumerable<Album>? albums = null;
        HttpResponseMessage response = await _client.GetAsync(AlbumsPath);
        if (response.IsSuccessStatusCode)
        {
            albums = await response.Content.ReadFromJsonAsync<IEnumerable<Album>>();
        }

        return albums ?? Enumerable.Empty<Album>();
    }

    public async Task<IEnumerable<Photo>> GetAlbumPhotos(int albumId)
    {
        var photosUrl = PhotosPath.AddQuery(nameof(albumId), albumId.ToString());
        IEnumerable<Photo>? photos = null;
        HttpResponseMessage response = await _client.GetAsync(photosUrl);
        if (response.IsSuccessStatusCode)
        {
            photos = await response.Content.ReadFromJsonAsync<IEnumerable<Photo>>();
        }

        return photos ?? Enumerable.Empty<Photo>();
    }
}

