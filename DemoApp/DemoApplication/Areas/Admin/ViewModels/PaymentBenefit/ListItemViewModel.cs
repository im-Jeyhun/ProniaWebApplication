namespace DemoApplication.Areas.Admin.ViewModels.PaymentBenefit
{
    public class ListItemViewModel
    {
        public ListItemViewModel(int id, string name, string imageName, string imageUrl, string content)
        {
            Id = id;
            Name = name;
            ImageName = imageName;
            ImageUrl = imageUrl;
            Content = content;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public string Content { get; set; }
    }
}
