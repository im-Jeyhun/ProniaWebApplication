using DemoApplication.Areas.Client.ViewModels.PlantItem;

namespace DemoApplication.Areas.Client.ViewModels.Product
{
    public class ListItemViewModel
    {
        public List<ListItemVIewModel> Products { get; set; }

        public List<ColorViewModel> Colors { get; set; }
        public List<SizeViewModel> Sizes { get; set; }
        public List<TagViewModel> Tags { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public List<SubCategoryViewModel> SubCategories { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string Search { get; set; }
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? TagId { get; set; }


        public class ColorViewModel
        {
            public ColorViewModel(int id, string naame)
            {
                Id = id;
                Naame = naame;
            }

            public int Id { get; set; }
            public string Naame { get; set; }

        }
        public class SizeViewModel
        {
            public SizeViewModel(int id, string naame)
            {
                Id = id;
                Naame = naame;
            }

            public int Id { get; set; }
            public string Naame { get; set; }
        }

        public class TagViewModel
        {
            public TagViewModel(int id, string naame)
            {
                Id = id;
                Naame = naame;
            }

            public int Id { get; set; }
            public string Naame { get; set; }
        }
        public class CategoryViewModel
        {
            public CategoryViewModel(int id, string title)
            {
                Id = id;
                Title = title;
            }

            public int Id { get; set; }
            public string Title { get; set; }
        }
        public class SubCategoryViewModel
        {
            public SubCategoryViewModel(int id, string naame)
            {
                Id = id;
                Naame = naame;
            }

            public int Id { get; set; }
            public string Naame { get; set; }
        }
    }
}
