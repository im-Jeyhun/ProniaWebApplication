namespace DemoApplication.Areas.Client.ViewModels.Blog
{
    public class BlogListItemViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }
        public List<TagViewModel> Tags { get; set; }

        public int? CategoryId { get; set; }
        public int? TagId { get; set; }
        public string Search { get; set; }
    }
}
