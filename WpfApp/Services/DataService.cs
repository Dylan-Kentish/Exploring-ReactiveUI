using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WpfApp.Model;


namespace WpfApp.Services;

public class DataService : IAlbumService
{
    private const string _uriString = "https://jsonplaceholder.typicode.com/";
    private const string _albumsPath = _uriString + "albums";


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
}

