using System.Collections.Generic;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
}