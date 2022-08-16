using System.Collections.Generic;
using System.Threading.Tasks;
using WpfApp.Services;

namespace WpfApp.Model;

public class User
{
    private readonly IAlbumService _albumService;

    public User(IAlbumService albumService, API.User user)
    {
        _albumService = albumService;
        Id = user.Id;
        Name = user.Name;
        Username = user.Username;
        Email = user.Email;
        Address = new Address(user.Address);
        Phone = user.Phone;
        Website = user.Website;
        Company = new Company(user.Company);
    }

    public int Id { get; }
    public string Name { get; }
    public string Username { get; }
    public string Email { get; }
    public Address Address { get; }
    public string Phone { get; }
    public string Website { get; }
    public Company Company { get; }

    public Task<IEnumerable<Album>> GetAlbums() => _albumService.GetUserAlbums(this);
}

