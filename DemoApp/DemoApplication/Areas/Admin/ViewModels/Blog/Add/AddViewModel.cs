using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.Blog.Add
{
    public class AddViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public Guid UserId { get; set; }

        public int BlogCategoryId { get; set; }

        public List<BlogCategorItemViewModel>? BlogCategiries { get; set; }

        [Required]
        public List<IFormFile>? BlogImages { get; set; }

        public List<IFormFile>? BlogVideos { get; set; }

        public string VideoUrl { get; set; }

        public List<int>? BlogTagIds { get; set; }

        public List<TagItemViewModel>? BlogTags { get; set; }


    }
}
