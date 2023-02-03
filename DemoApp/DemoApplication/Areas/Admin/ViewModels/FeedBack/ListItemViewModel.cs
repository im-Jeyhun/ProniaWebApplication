namespace DemoApplication.Areas.Admin.ViewModels.FeedBack
{
    public class ListItemViewModel
    {
        public int Id { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
