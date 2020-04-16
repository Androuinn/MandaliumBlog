using System.Collections.Generic;
using System.Threading.Tasks;
using Mandalium.API.Models;

namespace Mandalium.API.Data
{
    public interface IUserRepository
    {

        /// <summary>
        /// Updates the given writer with entity
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        Task<int> UpdateWriter(Writer writer);

        /// <summary>
        /// Gets all writers
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Writer>> GetWriters();
        /// <summary>
        /// Gets the writer specified with the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Writer> GetWriter(int id);
    }
}