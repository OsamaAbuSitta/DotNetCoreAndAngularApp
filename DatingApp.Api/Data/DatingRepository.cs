using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data
{
    public class DatingRepository : IDatingRepository
    {
        private DataContext _context { get; }
        public DatingRepository(DataContext context)
        {
            _context = context;

        }
        public void Add<TEntity>(TEntity obj) where TEntity : class
        {
            _context.Add(obj);
        }

        public void Delete<TEntity>(TEntity obj) where TEntity : class
        {
            _context.Remove(obj);
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.Include(u=> u.Photos).FirstOrDefaultAsync(u=> u.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Include(u => u.Photos).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
         return await _context.SaveChangesAsync() > 0 ;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return  await _context.Photos.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u => u.UserId == userId)
                .FirstOrDefaultAsync(p => p.IsMain);
        }
    }
}