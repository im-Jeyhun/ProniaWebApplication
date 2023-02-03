using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class Tag : BaseEntity<int> , IAuditable
    {
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<PlantTag>? PlantTags { get; set; }

    }
}
