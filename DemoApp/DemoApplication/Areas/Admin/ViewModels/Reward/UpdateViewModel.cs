namespace DemoApplication.Areas.Admin.ViewModels.Reward
{
    public class UpdateViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }

        public string ImageName { get; set; }   
        public IFormFile RewardImage { get; set; }
    }
}
