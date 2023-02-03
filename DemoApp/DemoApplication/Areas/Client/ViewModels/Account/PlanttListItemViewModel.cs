namespace DemoApplication.Areas.Client.ViewModels.Account
{
    public class PlanttListItemViewModel
    {
        public PlanttListItemViewModel(string? imageUrl, string name, decimal price, string color, string size, int? quantity, string orderId, decimal total)
        {
            ImageUrl = imageUrl;
            Name = name;
            Price = price;
            Color = color;
            Size = size;
            Quantity = quantity;
            OrderId = orderId;
            Total = total;
        }

        public string OrderId { get; set; }
        public string? ImageUrl { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int? Quantity { get; set; }
    }
}
