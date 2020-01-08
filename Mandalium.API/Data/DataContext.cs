using Mandalium.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Mandalium.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<BlogEntry> BlogEntries { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}