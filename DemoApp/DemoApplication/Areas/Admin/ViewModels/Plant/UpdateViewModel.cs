using DemoApplication.Areas.Admin.ViewModels.Plant.Add;
using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.Plant
{
    public class UpdateViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string? Content { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public int? SubCategoryId { get; set; }

        public List<int>? TagIds { get; set; }
        public List<int>? ColorIds { get; set; }
        public List<int>? SizeIds { get; set; }

        public List<Category.ListItemViewModel>? Categiries { get; set; }
        public List<SubCategory.ListItemViewModel>? SubCategories { get; set; }
        public List<TagListItemViewModel>? Tags { get; set; }
        public List<ColorListItemViewModel>? Colors { get; set; }

        public List<SizeListItemViewModel>? Sizes { get; set; }


       
    }
}
