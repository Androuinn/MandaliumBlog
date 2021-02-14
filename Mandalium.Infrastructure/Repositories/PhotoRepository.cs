using System.Linq;
using System.Threading.Tasks;
using Mandalium.Core.Context;
using Mandalium.Core.Helpers.Pagination;
using Mandalium.Core.Interfaces;
using Mandalium.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Mandalium.Infrastructure.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;
        public PhotoRepository(DataContext context)
        {
            this._context = context;

        }
        public async Task<int> AddPhoto(Photo photo)
        {
            await _context.Photos.AddAsync(photo);
            return await _context.SaveChangesAsync();
        }

        public async Task<PagedList<Photo>> GetPhotos(UserParams userParams)
        {
            var photos =  _context.Photos.AsNoTracking().Where(x=> x.User.Id == userParams.UserId).AsQueryable();

            return await PagedList<Photo>.CreateAsync(photos, userParams.PageNumber, userParams.PageSize);
        }
    }
}