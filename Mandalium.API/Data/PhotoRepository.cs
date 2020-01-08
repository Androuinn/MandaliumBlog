using System.Threading.Tasks;
using Mandalium.API.Dtos;
using Mandalium.API.Models;

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
    }
}