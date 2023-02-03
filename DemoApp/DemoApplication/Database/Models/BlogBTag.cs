using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class BlogBTag : BaseEntity<int>
    {
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public int BTagId { get; set; }
        public BTag BTag { get; set; }
    }
}
