namespace DemoApplication.Database.Models
{
    public class BlogVideo
    {
        public int Id { get; set; }
        public string VideoName { get; set; }
        public string VideoNameInFileSystem { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
