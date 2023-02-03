﻿using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class Color : BaseEntity<int> , IAuditable
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<PlantColor>? PlantColors { get; set; }
        public List<BasketProduct>? BasketProducts { get; set; }

    }
}
