namespace DemoApplication.Areas.Client.ViewModels.Home.Index
{
    public class PlantListItemViewModel
    {
        public PlantListItemViewModel(int id, string name, decimal price, int rate, string color, string size, string content, string mainImageUrl, string hoverImageUrl)
        {
            Id = id;
            Name = name;
            Price = price;
            Rate = rate;
            Color = color;
            Size = size;
            Content = content;
            MainImageUrl = mainImageUrl;
            HoverImageUrl = hoverImageUrl;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int Rate { get; set; }

        public string Color { get; set; }
        public string Size { get; set; }

        public string Content { get; set; }

        public string MainImageUrl { get; set; }
        public string HoverImageUrl { get; set; }
    }
}
