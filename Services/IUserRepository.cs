using System.Collections.Generic;
using System.Threading.Tasks;

using EventManager.API.Domain.Entities;

namespace EventManager.API.Services
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync ();
        void AddUser (User user);
        Task<bool> SaveChangesAsync ();

        bool UserExists(string username, string email);
    }
}
