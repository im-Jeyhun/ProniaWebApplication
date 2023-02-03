namespace DemoApplication.Areas.Admin.ViewModels.Blog
{
    public class ListItemViewModel
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string UserFullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string VideoUrl { get; set; }


    }
}
