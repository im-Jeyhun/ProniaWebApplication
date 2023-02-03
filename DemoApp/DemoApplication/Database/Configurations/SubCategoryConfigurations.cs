using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DemoApplication.Database.Configurations
{
    public class SubCategoryConfigurations : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder
               .ToTable("SubCategories");

            builder
               .HasOne(sc => sc.Category)
               .WithMany(c => c.SubCategories)
               .HasForeignKey(sc => sc.CategoryId);
        }
    }
}
