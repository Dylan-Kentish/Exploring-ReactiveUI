using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfApp.Model;


namespace WpfApp.Services;

public class DataService : IAlbumService, IPhotoService
{
    private const string _uriString = "https://jsonplaceholder.typicode.com/";
    private const string _albumsPath = _uriString + "albums";
    private const string _photosPath = _uriString + "photos";


    private readonly HttpClient _client;

    public DataService()
    {
        _client = new HttpClient();

        _client.BaseAddress = new Uri(_uriString);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<IEnumerable<Album>> GetAlbums()
    {
        IEnumerable<Album>? albums = null;
        HttpResponseMessage response = await _client.GetAsync(_albumsPath);
        if (response.IsSuccessStatusCode)
        {
            albums = await response.Content.ReadFromJsonAsync<IEnumerable<Album>>();
        }

        return albums ?? Enumerable.Empty<Album>();
    }

    public async Task<IEnumerable<Photo>> GetAlbumPhotos(int albumId)
    {
        var photosUrl = AddQuery(_photosPath, nameof(albumId), albumId.ToString());
        IEnumerable<Photo>? photos = null;
        HttpResponseMessage response = await _client.GetAsync(photosUrl);
        if (response.IsSuccessStatusCode)
        {
            photos = await response.Content.ReadFromJsonAsync<IEnumerable<Photo>>();
        }

        return photos ?? Enumerable.Empty<Photo>();
    }

    private string AddQuery(string url, string queryProperty, string queryValue)
    {
        if (url.Contains('?'))
        {
            return url + $"&{queryProperty}={queryValue}";
        }
        return url + $"?{queryProperty}={queryValue}";
    }
}

