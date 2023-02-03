namespace DemoApplication.Areas.Admin.ViewModels.SubNavbar
{
    public class ListItemViewModel
    {
        public ListItemViewModel(int id, string? name, string? url, string? navbarName, int rowNumber)
        {
            Id = id;
            Name = name;
            Url = url;
            NavbarName = navbarName;
            RowNumber = rowNumber;
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Url { get; set; }
        public string? NavbarName { get; set; }

        public int RowNumber { get; set; }

    }
}
