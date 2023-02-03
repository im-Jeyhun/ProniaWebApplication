using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DemoApplication.Database.Configurations
{
    public class BasketProductConfigurations : IEntityTypeConfiguration<BasketProduct>
    {
        public void Configure(EntityTypeBuilder<BasketProduct> builder)
        {
            builder
                .ToTable("BasketProducts");

            builder
              .HasOne(bp => bp.Basket)
              .WithMany(basket => basket.BasketProducts)
              .HasForeignKey(bp => bp.BasketId);

            builder
              .HasOne(bp => bp.Plant)
              .WithMany(plant => plant.BasketProducts)
              .HasForeignKey(bp => bp.PlantId);

        }
    }
}
