using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DemoApplication.Database.Configurations
{
    public class OrderProductConfigurations : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder
                .ToTable("OrderProducts");

            builder
              .HasOne(op => op.Order)
              .WithMany(order => order.OrderProducts)
              .HasForeignKey(op => op.OrderId);

            builder
             .HasOne(op => op.Plant)
             .WithMany(plant => plant.OrderProducts)
             .HasForeignKey(op => op.PlantId);

        }
    }
}
