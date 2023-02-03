using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.SubNavbar
{
    public class UpdateViewModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Url { get; set; }

        public List<UrlViewModel>? Urls { get; set; }

        [Required]
        public int RowNumber { get; set; }

        public int NavbarId { get; set; }
        public List<NavbarViewModel>? Navbars { get; set; }


        public class NavbarViewModel
        {
            public NavbarViewModel(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class UrlViewModel
        {
            public UrlViewModel(string? path)
            {
                Path = path;
            }

            public string? Path { get; set; }
        }
    }
}
