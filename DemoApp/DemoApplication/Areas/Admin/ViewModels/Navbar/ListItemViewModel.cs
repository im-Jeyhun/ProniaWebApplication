namespace DemoApplication.Areas.Admin.ViewModels.Navbar
{
    public class ListItemViewModel
    {
        public ListItemViewModel(int id, string name, string url, int rowNumber, bool isFooter, bool isHeader)
        {
            Id = id;
            Name = name;
            Url = url;
            RowNumber = rowNumber;
            IsFooter = isFooter;
            IsHeader = isHeader;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int RowNumber { get; set; }

        public bool IsFooter { get; set; }

        public bool IsHeader { get; set; }
    }
}
