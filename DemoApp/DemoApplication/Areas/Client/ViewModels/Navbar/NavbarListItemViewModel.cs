namespace DemoApplication.Areas.Client.ViewModels.Navbar
{
    public class NavbarListItemViewModel
    {
        public NavbarListItemViewModel(string? name, string? url, List<SubnavbarViewModel>? subnavbars)
        {
            Name = name;
            Url = url;
            Subnavbars = subnavbars;
        }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public List<SubnavbarViewModel>? Subnavbars { get; set; }

        public class SubnavbarViewModel
        {
            public SubnavbarViewModel(string name, string url)
            {
                Name = name;
                Url = url;
            }

            public string Name { get; set; }

            public string Url { get; set; }
        }
    }
}
