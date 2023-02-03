using DemoApplication.Contracts.User;
using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class FeedBack : BaseEntity<int> , IAuditable
    {
        public string ProfilPhotoName { get; set; }
        public string ProfilPhotoNameInFileSystem { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public MockRole Role { get; set; }
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
     
    }
}
