using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using EventManager.API.Domain.Entities;

namespace EventManager.API.Services
{
    public interface IUserRepository
    {
        Task<bool> SaveChangesAsync ();
        Task<bool> UserExistsAsync(string username, string email);
        Task<IEnumerable<User>> GetUsersAsync ();
        void AddUser (User user);
        Task<User> AuthenticateUserAsync (string username, string password);
        Task<User> GetUserByIdAsync(Guid userId);
    }
}
