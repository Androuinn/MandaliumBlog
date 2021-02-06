using Mandalium.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Mandalium.Core.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
          
        }

        public DbSet<BlogEntry> BlogEntries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<MostReadEntries> MostReadEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MostReadEntries>().HasNoKey();
            modelBuilder.Entity<MostReadEntries>().Property(x=> x.CreatedOn).ValueGeneratedOnAdd();
        }
    }
}