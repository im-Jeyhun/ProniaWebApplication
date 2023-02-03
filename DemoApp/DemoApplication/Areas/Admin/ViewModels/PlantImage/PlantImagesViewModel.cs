namespace DemoApplication.Areas.Admin.ViewModels.PlantImage
{
    public class PlantImagesViewModel
    {
        public string PlantName { get; set; }
        public int PlantId { get; set; }
        public List<ListItem>? Images { get; set; }





        public class ListItem
        {
            public int Id { get; set; }
            public string? ImageUrL { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
