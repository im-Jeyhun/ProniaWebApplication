using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class PaymentBenefit : BaseEntity<int>, IAuditable
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string ImageNameInFileSystem { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
