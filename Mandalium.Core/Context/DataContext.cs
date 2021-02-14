using Mandalium.Core.Configurations;
using Mandalium.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<MostReadEntries>().HasNoKey();
            modelBuilder.Entity<MostReadEntries>().Property(x=> x.CreatedOn).ValueGeneratedOnAdd();

            modelBuilder.Entity<Photo>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<Photo>().Property(p => p.PhotoUrl).IsRequired().HasColumnType("varchar(300)");
            modelBuilder.Entity<Photo>().Property(p => p.PublicId).IsRequired().HasColumnType("varchar(150)");
            modelBuilder.Entity<Photo>().HasOne(p => p.User).WithMany(p => p.Photos);
           
            modelBuilder.Entity<Topic>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<Topic>().Property(p => p.TopicName).IsRequired().HasColumnType("varchar(100)");


            modelBuilder.Entity<SystemSetting>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<SystemSetting>().Property(p => p.Key).IsRequired().HasColumnType("varchar(200)");
            modelBuilder.Entity<SystemSetting>().Property(p => p.Value).IsRequired().HasColumnType("varchar(200)");
        }
    }
}