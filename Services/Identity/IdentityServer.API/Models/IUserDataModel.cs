using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.API.Domains;

namespace IdentityServer.API.Models
{
    public interface IUserDataModel
    {
        Task<List<User>> GetUsersList();
        Task<User> GetUser(string username);
        Task InsertUser(User item);
        Task UpdateUser(User item);
        Task DeleteUser(string username);
    }
}
