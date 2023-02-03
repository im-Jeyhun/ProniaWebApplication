namespace DemoApplication.Areas.Admin.ViewModels.Plant.Add
{
    public class TagListItemViewModel
    {
        public TagListItemViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
