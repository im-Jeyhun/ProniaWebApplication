﻿using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApplication.Database.Configurations
{
    public class PaymentBenefitConfigurations : IEntityTypeConfiguration<PaymentBenefit>
    {
        public void Configure(EntityTypeBuilder<PaymentBenefit> builder)
        {
            builder
                .ToTable("PaymentBenefits");
        }
    }
}
