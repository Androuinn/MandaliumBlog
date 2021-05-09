using Mandalium.Core.Context;
using Mandalium.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandalium.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal DataContext _context;
        internal DbSet<T> dbSet;

        public GenericRepository(DataContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }


        public async Task Delete(int id)
        {
            T entityToDelete = await dbSet.FindAsync(id);
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
                dbSet.Attach(entityToDelete);
            dbSet.Remove(entityToDelete);
        }

        public async Task<T> Get(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task Save(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task Update(T entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
