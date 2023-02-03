namespace DemoApplication.Areas.Admin.ViewModels.BlogImage
{
    public class BlogImagesViewModel
    {
        public string BlogName { get; set; }
        public int BlogId { get; set; }
        public List<ListItem>? Images { get; set; }





        public class ListItem
        {
            public int Id { get; set; }
            public string? ImageUrL { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
