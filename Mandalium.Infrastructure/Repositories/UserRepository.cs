using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mandalium.Core.Context;
using Mandalium.Core.Interfaces;
using Mandalium.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Mandalium.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            this._context = context;

        }
        public async Task<int> UpdateUser(User user)
        {
            user.PhotoUrl = user.PhotoUrl.Split(".webp").First();
            _context.Entry(user).Property(x => x.Name).IsModified = true;
            _context.Entry(user).Property(x => x.Surname).IsModified = true;
            _context.Entry(user).Property(x => x.Background).IsModified = true;
            _context.Entry(user).Property(x => x.BirthDate).IsModified = true;
            _context.Entry(user).Property(x => x.PhotoUrl).IsModified = true;
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.AsNoTracking().Where(x => x.Role == true).ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}