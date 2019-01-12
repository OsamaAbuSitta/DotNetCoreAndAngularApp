using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Api.Helper;
using DatingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data
{
    public class AuthRepository : IAuthRepository
    {
        public DataContext _dataContext { get; }

        public AuthRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;

        }

        public async Task<User> Login(string username, string password)
        {
            User user = await _dataContext.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null) return null;

            if (!VarifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;

            return user;
        }

        private bool VarifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
                 return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))
                            .SequenceEqual(passwordHash);
        }

        public async Task<User> Register(User user, string password)
        {
            var passwordHash = Hashing.CreatePasswordHash(password);
            user.PasswordHash = passwordHash.passwordHash;
            user.PasswordSalt = passwordHash.passwordSalt;

            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }
        public async Task<bool> UserExists(string username)
        {
            return await _dataContext.Users.AnyAsync(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}