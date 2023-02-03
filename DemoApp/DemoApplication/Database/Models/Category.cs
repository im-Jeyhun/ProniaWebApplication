using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class Category : BaseEntity<int>, IAuditable
    {
        public string Title { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<SubCategory>? SubCategories { get; set; }

        public List<Plant>? Plants { get; set; }
    }
}
