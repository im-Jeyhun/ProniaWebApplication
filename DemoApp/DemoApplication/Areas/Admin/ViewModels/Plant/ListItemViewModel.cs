namespace DemoApplication.Areas.Admin.ViewModels.Plant
{
    public class ListItemViewModel
    {
        public int Id { get; set; } 
        public string Sku { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
