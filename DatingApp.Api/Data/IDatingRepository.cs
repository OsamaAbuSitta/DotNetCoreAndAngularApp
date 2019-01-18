using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Api.Helper;
using DatingApp.Api.Models;

namespace DatingApp.Api.Data
{
    public interface IDatingRepository
    {
        void Add<TEntity>(TEntity obj) where TEntity : class;
        void Delete<TEntity>(TEntity obj) where TEntity : class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
        Task<Like> GetLike(int userId, int recipientId);
    }
}