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
    public class BlogConfiguration : IEntityTypeConfiguration<BlogEntry>
    {
        public void Configure(EntityTypeBuilder<BlogEntry> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Headline).IsRequired().HasColumnType("varchar(200)");
            builder.Property(p => p.SubHeadline).IsRequired().HasColumnType("varchar(500)");
            builder.Property(p => p.InnerTextHtml).IsRequired().HasColumnType("varchar(MAX)");
            builder.Property(p => p.PhotoUrl).HasColumnType("varchar(250)");

            builder.HasMany(p => p.Comments).WithOne(p=> p.BlogEntry);
            builder.HasOne(p => p.User).WithMany(p=> p.BlogEntries);
            builder.HasOne(p => p.Topic);
            builder.Property(x => x.CreatedOn).ValueGeneratedOnAdd().HasDefaultValueSql("GetDate()");
           

        }

       

    }
}
