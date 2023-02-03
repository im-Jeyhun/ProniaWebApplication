namespace DemoApplication.Areas.Admin.ViewModels.Blog
{
    public class TagItemViewModel
    {
        public TagItemViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
