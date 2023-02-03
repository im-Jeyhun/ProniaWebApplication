using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class PlantColor : BaseEntity<int> 
    {
        public int PlantId { get; set; }
        public Plant? Plant { get; set; }

        public int ColorId { get; set; }
        public Color? Color { get; set; }
    }
}
