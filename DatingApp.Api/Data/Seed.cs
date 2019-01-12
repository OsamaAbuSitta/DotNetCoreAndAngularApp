using System.Collections.Generic;
using DatingApp.Api.Helper;
using DatingApp.Api.Models;

namespace DatingApp.Api.Data
{
    public class Seed
    {
        private DataContext _context { get; }
        public Seed(DataContext context)
        {
            _context = context;
        }

        public void SeedUserData()
        {
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(userData);

            foreach (var user in users)
            {
                var passwordHash = Hashing.CreatePasswordHash("123456");
                user.PasswordHash = passwordHash.passwordHash;
                user.PasswordSalt = passwordHash.passwordSalt;

                _context.Users.Add(user);
            }
            _context.SaveChanges();
        }
    }
}