using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using Mandalium.API.Dtos;
using Mandalium.API.Helpers;
using Mandalium.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mandalium.API.Data
{
    public class BlogRepository : IBlogRepository<BlogEntry>
    {
        private readonly DataContext _context;
        public BlogRepository(DataContext context)
        {
            this._context = context;
        }

        #region  Get methods
        public async Task<PagedList<BlogEntry>> GetBlogEntries(UserParams userParams)
        {
            IQueryable<BlogEntry> blogEntries;
            if (userParams.WriterId >= 1)
            {
                blogEntries = _context.BlogEntries.AsNoTracking().Include(x => x.Topic).Include(x => x.Writer).Where(x => x.WriterId == userParams.WriterId).OrderByDescending(x => x.CreatedDate).AsQueryable();
            }
            else
            {
                blogEntries = _context.BlogEntries.AsNoTracking().Include(x => x.Topic).Include(x => x.Writer).Where(x => x.WriterEntry == userParams.WriterEntry).OrderByDescending(x => x.CreatedDate).AsQueryable();
            }

            return await PagedList<BlogEntry>.CreateAsync(blogEntries, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<BlogEntry> GetBlogEntry(int blogId, UserParams userParams)
        {
            if (userParams.EntryAlreadyPicked == false)
            {
                var Entry = await _context.BlogEntries.AsNoTracking().Include(x => x.Writer).Include(x => x.Topic).FirstOrDefaultAsync(x => x.Id == blogId);
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
            var a = Counter.GetMostRead().SelectMany(x => x.Value).OrderByDescending(x => x.Count).Where(x => x.WriterEntry == writerEntry).Take(5).ToList();
            //order by hallet
            if (a.Count != 0)
            {
                Entries = await _context.BlogEntries.Where(x => a.Select(c => c.Id).Contains(x.Id)).ToListAsync();
            }
            else
            {
                Entries = await _context.BlogEntries.OrderByDescending(x => x.TimesRead).Where(x => x.WriterEntry == writerEntry).Take(5).ToListAsync();
            }

            return Entries;
        }

        #endregion

        public async Task<PagedList<BlogEntry>> Search(string searchString, UserParams userParams)
        {
            var blogEntries = from e in _context.BlogEntries.AsNoTracking().Include(x => x.Topic).OrderByDescending(x => x.CreatedDate).AsQueryable() select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                blogEntries = blogEntries.Where(x => x.Headline.Contains(searchString) || x.SubHeadline.Contains(searchString) || x.innerTextHtml.Contains(searchString));
            }

            return await PagedList<BlogEntry>.CreateAsync(blogEntries, userParams.PageNumber, userParams.PageSize);
        }


        #region saving
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

        public async Task<int> SaveComment(Comment comment)
        {
            await _context.AddAsync(comment);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveTopic(Topic topic)
        {
            await _context.AddAsync(topic);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Topic and Writer
        public async Task<IEnumerable<Topic>> GetTopics()
        {
            return await _context.Topics.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Writer>> GetWriters()
        {
            return await _context.Writers.AsNoTracking().ToListAsync();
        }

        #endregion
    }
}