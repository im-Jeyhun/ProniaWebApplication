using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DemoApplication.Database.Configurations
{
    public class PlantTagConfigurations : IEntityTypeConfiguration<PlantTag>
    {
        public void Configure(EntityTypeBuilder<PlantTag> builder)
        {
            builder
               .ToTable("PlantTags")
               .HasOne(pt => pt.Plant)
               .WithMany(p => p.PlantTags)
               .HasForeignKey(pt => pt.PlantId);
        }
    }
}
