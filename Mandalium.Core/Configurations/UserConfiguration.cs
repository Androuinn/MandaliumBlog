using Mandalium.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandalium.Core.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Username).IsRequired().HasColumnType("varchar(50)");
            builder.Property(p => p.Name).IsRequired().HasColumnType("varchar(50)");
            builder.Property(p => p.Surname).IsRequired().HasColumnType("varchar(50)");
            builder.Property(p => p.Email).IsRequired().HasColumnType("varchar(100)");
            builder.Property(p => p.PhotoUrl).HasColumnType("varchar(300)");

            builder.HasMany(p => p.Comments).WithOne(p => p.User);
            builder.HasMany(p => p.Photos).WithOne(p => p.User);
            builder.HasMany(p => p.BlogEntries).WithOne(p=> p.User);
            builder.Property(x => x.CreatedOn).ValueGeneratedOnAdd().HasDefaultValueSql("GetDate()");
        }
    }
}
