namespace DemoApplication.Areas.Admin.ViewModels.BlogImage
{
    public class UpdateViewModel
    {
        public int BlogId { get; set; }
        public int BlogImageeId { get; set; }

        public string ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
