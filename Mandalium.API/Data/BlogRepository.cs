using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mandalium.API.Helpers;
using Mandalium.API.Models;
using Microsoft.EntityFrameworkCore;


//TODO writer ile ilgili olanları user repo ve controller altında topla
namespace Mandalium.API.Data
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

        public async Task<BlogEntry> GetBlogEntry(int blogId, UserParams userParams)
        {
            if (userParams.EntryAlreadyPicked == false)
            {
                var Entry = await _context.BlogEntries.AsNoTracking().Include(x => x.User).Include(x => x.Topic).FirstOrDefaultAsync(x => x.Id == blogId);
                var comments = _context.Comments.AsNoTracking().Where(x => x.BlogEntryId == blogId).OrderByDescending(x => x.CreatedDate).AsQueryable();
                Entry.Comments = await PagedList<Comment>.CreateAsync(comments, userParams.PageNumber, userParams.PageSize);


                Counter.Add(DateTime.Now.Date, blogId, Entry.WriterEntry);

                Entry.TimesRead += 1;
                _context.BlogEntries.Update(Entry);
                await _context.SaveChangesAsync();
                return Entry;
            }
            else
            {
                BlogEntry BlogEntry = new BlogEntry();
                var comments = _context.Comments.AsNoTracking().Where(x => x.BlogEntryId == blogId).OrderByDescending(x => x.CreatedDate).AsQueryable();
                BlogEntry.Comments = await PagedList<Comment>.CreateAsync(comments, userParams.PageNumber, userParams.PageSize);

                return BlogEntry;
            }
        }

        public async Task<IEnumerable<BlogEntry>> GetMostRead(bool writerEntry)
        {

            List<BlogEntry> Entries;
            var a = Counter.GetMostRead().SelectMany(x => x.Value).OrderByDescending(x => x.Count).Where(x => x.WriterEntry == writerEntry).ToList();
            //order by hallet
            if (a.Count != 0)
            {
                Entries = await _context.BlogEntries.Where(x => (a.Select(c => c.Id).Contains(x.Id)) && (x.isDeleted == false)).Take(5).ToListAsync();
            }
            else
            {
                Entries = await _context.BlogEntries.OrderByDescending(x => x.TimesRead).Where(x => (x.WriterEntry == writerEntry) && (x.isDeleted == false)).Take(5).ToListAsync();
            }

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