using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class SubCategory : BaseEntity<int>, IAuditable
    {

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Title { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<Plant>? Plants { get; set; }

    }
}
