using System.Collections.Generic;
using System.Threading.Tasks;
using Mandalium.Core.Helpers.Pagination;
using Mandalium.Core.Models;

namespace Mandalium.Core.Interfaces
{
    public interface IBlogRepository<T>
    {   

        /// <summary>
        /// Gets all Blog Entries given by the user parameters.
        /// </summary>
        /// <param name="userParams"></param>
        /// <returns></returns>
        Task<PagedList<T>> GetBlogEntries(UserParams userParams);

        /// <summary>
        /// Gets the Blog Entry with the given Id and gets the Comments specified with the user parameters.
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="userParams"></param>
        /// <returns></returns>
        Task<T> GetBlogEntry(int blogId);
        Task<PagedList<Comment>> GetComments(int id, UserParams userParams);

        /// <summary>
        /// Gets Most Read Blog Entries(writerEntry specifies if it is user specific or not)
        /// </summary>
        /// <param name="writerEntry"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetMostRead();

    }
}