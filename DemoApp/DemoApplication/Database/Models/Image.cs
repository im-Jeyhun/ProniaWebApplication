using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class Image : BaseEntity<int>
    {
        public int Order { get; set; }
        public string ImageName { get; set; }
        public string ImageNameInFileSystem { get; set; }

        public int PlantId { get; set; }
        public Plant Plant { get; set; }


    }
}
