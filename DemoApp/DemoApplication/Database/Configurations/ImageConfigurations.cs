using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DemoApplication.Database.Configurations
{
    public class ImageConfigurations : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder
            .ToTable("Images")
               .HasOne(i => i.Plant)
               .WithMany(p => p.Images)
               .HasForeignKey(i => i.PlantId);
        }
    }
}
