using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EventManager.API.Data;
using EventManager.API.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventManager.API.Services
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private DataContext _dataContext;

        public UserRepository (DataContext dataContext, ILogger<UserRepository> logger)
        {
            _dataContext = dataContext ??
                throw new ArgumentNullException (nameof (dataContext));
        }
        public void AddUser (User userToAdd)
        {
            if (userToAdd == null)
            {
                throw new ArgumentNullException (nameof (userToAdd));
            }

            _dataContext.Add (userToAdd);
        }

        public async Task<IEnumerable<User>> GetUsersAsync ()
        {
            return await _dataContext.Users.ToListAsync ();
        }

        public async Task<User> AuthenticateUserAsync (string username, string password)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync (x => x.Username == username || x.Email == username);

            if (user == null) return null;

            var isUserAuthenticated = BCrypt.Net.BCrypt.Verify (password, user.Password);

            if (!isUserAuthenticated) return null;

            return user;
        }

        public async Task<User> GetUserByIdAsync (Guid userId)
        {
            var user = await _dataContext.Users.FindAsync (userId);

            if (user == null) return null;

            return user;
        }
        public async Task<bool> SaveChangesAsync ()
        {
            return (await _dataContext.SaveChangesAsync () > 0);
        }

        public async Task<bool> UserExistsAsync (string username, string email)
        {
            return await _dataContext.Users.AnyAsync (u => u.Username == username || u.Email == email);
        }

        public void Dispose ()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        protected virtual void Dispose (bool disposing)
        {
            if (disposing)
            {
                if (_dataContext != null)
                {
                    _dataContext.Dispose ();
                    _dataContext = null;
                }
            }
        }
    }
}
