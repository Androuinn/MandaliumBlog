using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mandalium.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Mandalium.API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            this._context = context;

        }
        public async Task<int> UpdateWriter(Writer writer)
        {
            writer.PhotoUrl = writer.PhotoUrl.Split(".webp").First();
            _context.Entry(writer).Property(x => x.Name).IsModified = true;
            _context.Entry(writer).Property(x => x.Surname).IsModified = true;
            _context.Entry(writer).Property(x => x.Background).IsModified = true;
            _context.Entry(writer).Property(x => x.BirthDate).IsModified = true;
            _context.Entry(writer).Property(x => x.PhotoUrl).IsModified = true;
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Writer>> GetWriters()
        {
            return await _context.Writers.AsNoTracking().Where(x => x.Role == true).ToListAsync();
        }

        public async Task<Writer> GetWriter(int id)
        {
            return await _context.Writers.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}