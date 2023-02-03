namespace DemoApplication.Areas.Client.ViewModels.Blog
{
    public class BlogItemViewModel
    {
        public int Id { get; set; }
        public string User { get; set; }
        public DateTime CreatedAt { get; set; }
        public string  Title { get; set; }
        public string Content { get; set; }
        public string VideoUrl { get; set; }
        public string ImageUrl { get; set; }

        public List<TagViewModel> Tags { get; set; }
    }
}
