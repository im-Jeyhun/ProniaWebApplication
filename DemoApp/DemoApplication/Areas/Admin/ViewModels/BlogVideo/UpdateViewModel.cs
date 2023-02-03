namespace DemoApplication.Areas.Admin.ViewModels.BlogVideo
{
    public class UpdateViewModel
    {
        public int BlogId { get; set; }
        public int BlogtVideoId { get; set; }

        public string VideoUrl { get; set; }
        public IFormFile? Video { get; set; }
    }
}
