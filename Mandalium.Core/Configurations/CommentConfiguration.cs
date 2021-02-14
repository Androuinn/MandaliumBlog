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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.CommenterName).HasColumnType("varchar(100)");
            builder.Property(p => p.Email).HasColumnType("varchar(100)");
            builder.Property(p => p.CommentString).IsRequired().HasColumnType("varchar(500)");
          

            builder.HasOne(p => p.BlogEntry).WithMany(p => p.Comments).IsRequired();
            builder.HasOne(p => p.User).WithMany(p => p.Comments);
            builder.Property(x => x.CreatedDate).ValueGeneratedOnAdd();
        }
    }
}
