using System.Linq;
using System.Threading.Tasks;
using Mandalium.API.Dtos;
using Mandalium.API.Helpers;
using Mandalium.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Mandalium.API.Data
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
            var photos =  _context.Photos.AsNoTracking().Where(x=> x.WriterId == userParams.WriterId).AsQueryable();

            return await PagedList<Photo>.CreateAsync(photos, userParams.PageNumber, userParams.PageSize);
        }
    }
}