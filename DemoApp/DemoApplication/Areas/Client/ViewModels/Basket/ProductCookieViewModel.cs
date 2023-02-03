namespace DemoApplication.Areas.Client.ViewModels.Basket
{
    public class ProductCookieViewModel
    {
        public ProductCookieViewModel(int id, string? name, string? imageUrl, int? quantity, decimal? price, decimal? total)
        {
            Id = id;
            Name = name;
            ImageUrl = imageUrl;
            Quantity = quantity;
            Price = price;
            Total = total;
        }

        public ProductCookieViewModel(int id, string? name, string? imageUrl, int? quantity, decimal? price, decimal? total, int? colorId, int? sizeId, List<SizeItemViewModel> sizeItemViewModels)
        {
            Id = id;
            Name = name;
            ImageUrl = imageUrl;
            Quantity = quantity;
            Price = price;
            Total = total;
            ColorId = colorId;
            SizeId = sizeId;
        }

        public ProductCookieViewModel()
        {

        }

        public ProductCookieViewModel(int id, string? name, string? imageUrl, int? quantity, decimal? price, decimal? total, int? colorId, int? sizeId, List<ColorItemViewModel> colors, List<SizeItemViewModel> sizes)
        {
            Id = id;
            Name = name;
            ImageUrl = imageUrl;
            Quantity = quantity;
            Price = price;
            Total = total;
            ColorId = colorId;
            SizeId = sizeId;
            Colors = colors;
            Sizes = sizes;
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }
        public int? ColorId { get; set; }

        public int? SizeId { get; set; }

        public List<ColorItemViewModel>? Colors { get; set; }
        public List<SizeItemViewModel>? Sizes { get; set; }

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
