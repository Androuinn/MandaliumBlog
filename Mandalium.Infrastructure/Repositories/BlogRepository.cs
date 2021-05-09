using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mandalium.Core.Helpers;
using Mandalium.Core.Context;
using Mandalium.Core.Helpers.Pagination;
using Mandalium.Core.Interfaces;
using Mandalium.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Mandalium.Infrastructure.Repositories
{
    public class BlogRepository : GenericRepository<BlogEntry>, IBlogRepository<BlogEntry>
    {
        private readonly DataContext _context;
        public BlogRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        #region  Get methods for blog

        public async Task<PagedList<BlogEntry>> GetBlogEntries(UserParams userParams)
        {
            IQueryable<BlogEntry> blogEntries;
            if (userParams.UserId >= 1)
            {
                blogEntries = _context.BlogEntries.AsNoTracking().Include(x => x.Topic).Include(x => x.User).Where(x => x.User.Id == userParams.UserId).Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedOn).AsQueryable();
            }
            else
            {
                blogEntries = _context.BlogEntries.AsNoTracking().Include(x => x.Topic).Include(x => x.User).Where(x => x.WriterEntry == userParams.WriterEntry).Where(x => !x.IsDeleted ).OrderByDescending(x => x.CreatedOn).AsQueryable();
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
            var comments = _context.Comments.AsNoTracking().Include(x => x.User).Where(x => x.BlogEntry.Id == id).OrderByDescending(x => x.CreatedDate).AsQueryable();
            return await PagedList<Comment>.CreateAsync(comments, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<IEnumerable<BlogEntry>> GetMostRead()
        {
            List<BlogEntry> Entries = await _context.BlogEntries.FromSqlRaw("Exec GetMostReadEntries").ToListAsync();
            return Entries;
        }

        #endregion

    }
}