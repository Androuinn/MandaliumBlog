using System.Collections.Generic;
using System.Threading.Tasks;
using Mandalium.API.Helpers;
using Mandalium.API.Models;

namespace Mandalium.API.Data
{
    public interface IBlogRepository<T>
    {
        Task<PagedList<T>> GetBlogEntries(UserParams userParams);
        Task<T> GetBlogEntry(int blogId, UserParams userParams);

        Task<IEnumerable<T>> GetMostRead(bool writerEntry);

        Task<int> SaveBlogEntry(BlogEntry blogEntry);

        Task<IEnumerable<Writer>> GetWriters();
        Task<IEnumerable<Topic>> GetTopics();

        Task<int> SaveComment(Comment comment);
    }
}