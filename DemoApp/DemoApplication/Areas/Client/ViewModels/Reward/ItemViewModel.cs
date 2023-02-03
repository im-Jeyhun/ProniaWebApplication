namespace DemoApplication.Areas.Client.ViewModels.Reward
{
    public class ItemViewModel
    {
        public ItemViewModel(string imageName, string imageUrl)
        {
            ImageName = imageName;
            ImageUrl = imageUrl;
        }

        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
    }
}
