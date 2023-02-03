using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DemoApplication.Database.Configurations
{
    public class PlantSizeConfigurations : IEntityTypeConfiguration<PlantSize>
    {
        public void Configure(EntityTypeBuilder<PlantSize> builder)
        {
            builder
               .ToTable("PlantSizes")
               .HasOne(pc => pc.Plant)
               .WithMany(p => p.PlantSizes)
               .HasForeignKey(pc => pc.PlantId);

            builder
               .HasOne(pc => pc.Size)
               .WithMany(c => c.PlantSizes)
               .HasForeignKey(pc => pc.SizeId);
        }
    }
}
