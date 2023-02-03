namespace DemoApplication.Areas.Admin.ViewModels.BlogVideo
{
    public class BlogVideViewModel
    {
        public string BlogName { get; set; }
        public int BlogId { get; set; }
        public List<ListItem>? Videos { get; set; }





        public class ListItem
        {
            public int Id { get; set; }
            public string? ViedeoUrl { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
