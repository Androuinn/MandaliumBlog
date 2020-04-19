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
        Task<int> UpdateUser(User writer);

        /// <summary>
        /// Gets all writers
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetUsers();
        /// <summary>
        /// Gets the writer specified with the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> GetUser(int id);
    }
}