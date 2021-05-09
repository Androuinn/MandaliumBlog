using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandalium.Core.Interfaces
{
    public interface IGenericRepository<T> where T: class
    {
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();
        Task Delete(int id);
        Task Update(T entity);
        Task Save(T entity);


    }
}
