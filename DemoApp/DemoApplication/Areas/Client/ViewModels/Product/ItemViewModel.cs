using DemoApplication.Areas.Client.ViewModels.Plant;
using DemoApplication.Areas.Client.ViewModels.PlantItem;

namespace DemoApplication.Areas.Client.ViewModels.Product
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Content { get; set; }
        public int? Quantity { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int? SubCategoryId { get; set; }

        public string? SubCategoryName { get; set; }

        public int? ColorId { get; set; }
        public List<ColorItemViewModel> Colors { get; set; }
        public int? SizeId { get; set; }

        public List<SizeItemViewModel> Sizes { get; set; }
        public List<ImageItemViewModel> ImageUrls { get; set; }
        public List<TagItemViewModel> Tags { get; set; }

        public List<ListItemVIewModel> Products { get; set; }

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

        public class TagItemViewModel
        {
            public TagItemViewModel(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; set; }

            public string Name { get; set; }
        }
        public class ImageItemViewModel
        {
            public ImageItemViewModel(string imageName, string imageUrl)
            {
                ImageName = imageName;
                ImageUrl = imageUrl;
            }

            public string ImageName { get; set; }

            public string ImageUrl { get; set; }
        }

    }
}
