using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DemoApplication.Database.Configurations
{
    public class PlantConfigurations : IEntityTypeConfiguration<Plant>
    {
        public void Configure(EntityTypeBuilder<Plant> builder)
        {
            builder
               .ToTable("Plants");

            builder
            .HasOne(p => p.SubCategory)
            .WithMany(sc => sc.Plants)
            .HasForeignKey(p => p.SubCategoryId);

            builder
            .HasOne(p => p.Category)
            .WithMany(c => c.Plants)
            .HasForeignKey(p => p.CategoryId);
        }
    }
}
