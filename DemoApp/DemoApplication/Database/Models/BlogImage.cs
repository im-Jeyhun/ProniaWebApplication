namespace DemoApplication.Database.Models
{
    public class BlogImage
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string ImageNameInFileSystem { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
