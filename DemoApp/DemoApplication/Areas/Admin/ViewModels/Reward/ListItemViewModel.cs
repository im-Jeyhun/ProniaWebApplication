namespace DemoApplication.Areas.Admin.ViewModels.Reward
{
    public class ListItemViewModel
    {
        public ListItemViewModel(int id, string imageUrl)
        {
            Id = id;
            ImageUrl = imageUrl;
        }

        public int Id { get; set; }
        public string ImageUrl { get; set; }
    }
}
