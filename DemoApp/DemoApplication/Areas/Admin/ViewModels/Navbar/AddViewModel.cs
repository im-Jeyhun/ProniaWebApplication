using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.Navbar
{
    public class AddViewModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Url { get; set; }

        [Required]
        public int RowNumber { get; set; }

        [Required]
        public bool IsFooter { get; set; }

        [Required]
        public bool IsHeader { get; set; }
    }
}
