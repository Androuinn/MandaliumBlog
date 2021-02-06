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
        Task<T> GetBlogEntry(int blogId, UserParams userParams);
        Task<PagedList<Comment>> GetComments(int id, UserParams userParams);

        /// <summary>
        /// Gets Most Read Blog Entries(writerEntry specifies if it is user specific or not)
        /// </summary>
        /// <param name="writerEntry"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetMostRead(bool writerEntry);

        /// <summary>
        /// Creates a new blog entry with the given entity.
        /// </summary>
        /// <param name="blogEntry"></param>
        /// <returns></returns>
        Task<int> SaveBlogEntry(BlogEntry blogEntry);

        /// <summary>
        /// Updates the blog entry specified with the given entity.
        /// </summary>
        /// <param name="blogEntry"></param>
        /// <returns></returns>
        Task<int> UpdateBlogEntry(BlogEntry blogEntry);

        /// <summary>
        /// Updates the blog entry as deleted in the database with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteBlogEntry(int id);

        /// <summary>
        /// Saves a comment with the given entity.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<int> SaveComment(Comment comment);
        Task<int> SaveTopic(Topic topic);
        Task<IEnumerable<Topic>> GetTopics();
    }
}