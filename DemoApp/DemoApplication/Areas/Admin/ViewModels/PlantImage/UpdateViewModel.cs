namespace DemoApplication.Areas.Admin.ViewModels.PlantImage
{
    public class UpdateViewModel
    {
        public int PlantId { get; set; }
        public int PlantImageId { get; set; }

        public string ImageUrl { get; set; }
        public int Order { get; set; }
        public IFormFile? Image { get; set; }
    }
}
