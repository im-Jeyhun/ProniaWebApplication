using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class BlogCategory : BaseEntity<int>, IAuditable
    {
        public string Title { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Blog>? Blogs { get; set; }
    }
}
