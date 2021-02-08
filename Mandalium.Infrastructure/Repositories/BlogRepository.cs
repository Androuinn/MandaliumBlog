using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mandalium.API.Helpers;
using Mandalium.Core.Context;
using Mandalium.Core.Helpers.Pagination;
using Mandalium.Core.Interfaces;
using Mandalium.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


//TODO writer ile ilgili olanları user repo ve controller altında topla
namespace Mandalium.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository<BlogEntry>
    {
        private readonly DataContext _context;
        public BlogRepository(DataContext context)
        {
            this._context = context;
        }

        #region  Get methods for blog

        public async Task<PagedList<BlogEntry>> GetBlogEntries(UserParams userParams)
        {
            IQueryable<BlogEntry> blogEntries;
            if (userParams.UserId >= 1)
            {
                blogEntries = _context.BlogEntries.AsNoTracking().Include(x => x.Topic).Include(x => x.User).Where(x => x.UserId == userParams.UserId).Where(x => x.isDeleted == false).OrderByDescending(x => x.CreatedDate).AsQueryable();
            }
            else
            {
                blogEntries = _context.BlogEntries.AsNoTracking().Include(x => x.Topic).Include(x => x.User).Where(x => x.WriterEntry == userParams.WriterEntry).Where(x => x.isDeleted == false).OrderByDescending(x => x.CreatedDate).AsQueryable();
            }

            return await PagedList<BlogEntry>.CreateAsync(blogEntries, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<BlogEntry> GetBlogEntry(int blogId)
        {
          
                var Entry = await _context.BlogEntries.AsNoTracking().Include(x => x.User).Include(x => x.Topic).FirstOrDefaultAsync(x => x.Id == blogId);

                //TODO userId de eklenebilir.
                var Id = new SqlParameter("@BlogId", Entry.Id);
                var writerEntry = new SqlParameter("@WriterEntry", Entry.WriterEntry);
                await _context.Database.ExecuteSqlRawAsync("EXEC InsertMostRead @BlogId, @WriterEntry", Id, writerEntry);

                Entry.TimesRead += 1;
                _context.BlogEntries.Update(Entry);
                await _context.SaveChangesAsync();
                return Entry;
           
        }

        public async Task<PagedList<Comment>> GetComments(int id, UserParams userParams)
        {
            var comments = _context.Comments.AsNoTracking().Include(x => x.User).Where(x => x.BlogEntryId == id).OrderByDescending(x => x.CreatedDate).AsQueryable();
            return await PagedList<Comment>.CreateAsync(comments, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<IEnumerable<BlogEntry>> GetMostRead()
        {

            List<BlogEntry> Entries = await _context.BlogEntries.FromSqlRaw("Exec GetMostReadEntries").ToListAsync(); 
            return Entries;
        }

        #endregion


        #region saving includes Comments
        public async Task<int> SaveBlogEntry(BlogEntry blogEntry)
        {
            await _context.AddAsync(blogEntry);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateBlogEntry(BlogEntry blogEntry)
        {
            _context.Update(blogEntry);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteBlogEntry(int id)
        {
            var entry = await _context.BlogEntries.FirstOrDefaultAsync(x => x.Id == id);
            entry.isDeleted = true;
            _context.BlogEntries.Update(entry);
            return await _context.SaveChangesAsync();
        }




        public async Task<int> SaveComment(Comment comment)
        {
            await _context.AddAsync(comment);
            return await _context.SaveChangesAsync();
        }


        #endregion

        #region Topics
        public async Task<IEnumerable<Topic>> GetTopics()
        {
            return await _context.Topics.AsNoTracking().ToListAsync();
        }

        public async Task<int> SaveTopic(Topic topic)
        {
            await _context.AddAsync(topic);
            return await _context.SaveChangesAsync();
        }


        #endregion
    }
}