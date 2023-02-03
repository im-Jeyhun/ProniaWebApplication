using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class BTag : BaseEntity<int> , IAuditable
    {
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<BlogBTag>? BlogTags { get; set; }

    }
}
