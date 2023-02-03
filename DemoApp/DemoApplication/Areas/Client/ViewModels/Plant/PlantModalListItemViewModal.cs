namespace DemoApplication.Areas.Client.ViewModels.Plant
{
    public class PlantModalListItemViewModal
    {
     

        public PlantModalListItemViewModal(int id, string? imageUrl, string name, decimal price, string content, List<ColorItemViewModel> colors, List<SizeItemViewModel> sizes)
        {
            Id = id;
            ImageUrl = imageUrl;
            Name = name;
            Price = price;
            Content = content;
            Colors = colors;
            Sizes = sizes;
        }
        public PlantModalListItemViewModal()
        {

        }

        public PlantModalListItemViewModal(int? quantity, int? colorId, int? sizeId)
        {
            Quantity = quantity;
            ColorId = colorId;
            SizeId = sizeId;
        }
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Content { get; set; }
        public int? Quantity { get; set; }

        public int? ColorId { get; set; }
        public List<ColorItemViewModel> Colors { get; set; }
        public int? SizeId { get; set; }

        public List<SizeItemViewModel> Sizes { get; set; }

       
        public class ColorItemViewModel
        {
            public ColorItemViewModel(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; set; }
            public string Name { get; set; }


           
        }

        public class SizeItemViewModel
        {
            public SizeItemViewModel(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; set; }
            public string Name { get; set; }

        }

    }
}
