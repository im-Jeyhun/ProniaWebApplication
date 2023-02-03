using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class About : BaseEntity<int>, IAuditable
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
