namespace DemoApplication.Areas.Client.ViewModels.Blog
{
    public class TagViewModel
    {
        public TagViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
