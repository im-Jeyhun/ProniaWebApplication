using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class Plant : BaseEntity<int>, IAuditable
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int CategoryId { get; set; }  
        public Category? Category { get; set; }
        public int? SubCategoryId { get; set; } 
        public SubCategory? SubCategory { get; set; }
        public List<PlantSize>? PlantSizes { get; set; }
        public List<PlantColor>? PlantColors { get; set; }

        public List<BasketProduct>? BasketProducts { get; set; }
        public List<Image>? Images { get; set; }
        public List<PlantTag>? PlantTags { get; set; }
        public List<OrderProduct>? OrderProducts { get; set; }


    }
}
