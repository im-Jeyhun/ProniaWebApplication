namespace DemoApplication.Areas.Admin.ViewModels.Blog
{
    public class BlogCategorItemViewModel
    {
        public BlogCategorItemViewModel(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public int Id { get; set; }

        public string? Title { get; set; }
    }
}
