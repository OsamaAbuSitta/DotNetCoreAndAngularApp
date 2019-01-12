using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Api.Models;

namespace DatingApp.Api.Data
{
    public interface IDatingRepository
    {
        void Add<TEntity>(TEntity obj) where TEntity : class;
        void Delete<TEntity>(TEntity  obj)where TEntity : class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
         Task<Photo> GetMainPhotoForUser(int userId);
    }
}