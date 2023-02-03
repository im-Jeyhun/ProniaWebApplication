using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DemoApplication.Database.Configurations
{
    public class BlogBTagConfigurations : IEntityTypeConfiguration<BlogBTag>
    {
        public void Configure(EntityTypeBuilder<BlogBTag> builder)
        {
          
            builder
               .ToTable("BlogBTags")
               .HasOne(bt => bt.Blog)
               .WithMany(b => b.BlogTags)
               .HasForeignKey(bt => bt.BlogId);

            builder
                .HasOne(bt => bt.BTag)
               .WithMany(bt => bt.BlogTags)
               .HasForeignKey(bt => bt.BTagId);
        }
    }
}
