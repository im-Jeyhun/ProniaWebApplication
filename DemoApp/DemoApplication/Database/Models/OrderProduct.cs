using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class OrderProduct : BaseEntity<int>, IAuditable
    {
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string OrderId { get; set; }
        public int? Quantity { get; set; }
        public Order? Order { get; set; }
        public int PlantId { get; set; }
        public Plant? Plant { get; set; }

        public string Color { get; set; }
        public string Size { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
