namespace DemoApplication.Areas.Admin.ViewModels.BlogTag
{
    public class ListItemViewModel
    {
        public ListItemViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
