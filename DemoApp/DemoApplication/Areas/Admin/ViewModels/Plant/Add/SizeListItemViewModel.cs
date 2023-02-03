namespace DemoApplication.Areas.Admin.ViewModels.Plant.Add
{
    public class SizeListItemViewModel
    {
        public SizeListItemViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
