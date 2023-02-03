namespace DemoApplication.Areas.Admin.ViewModels.SubCategory
{
    public class ListItemViewModel
    {
        public ListItemViewModel(int id, string title, string categoryName)
        {
            Id = id;
            Title = title;
            CategoryName = categoryName;
        }

        public int Id { get; set; }

        public string? Title { get; set; }

        public string CategoryName { get; set; }
    }
}
