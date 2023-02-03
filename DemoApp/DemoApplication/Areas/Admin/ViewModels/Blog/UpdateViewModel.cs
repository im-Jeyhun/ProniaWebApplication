using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.Blog
{
    public class UpdateViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string? Title { get; set; }

        public string VideoUrl { get; set; }


        [Required]
        public string? Content { get; set; }

        [Required]
        public int BlogCategoryId { get; set; }

        public List<int>? TagIds { get; set; }

        public List<BlogCategorItemViewModel>? BlogCategiries { get; set; }
        public List<TagItemViewModel>? Tags { get; set; }

    }
}
