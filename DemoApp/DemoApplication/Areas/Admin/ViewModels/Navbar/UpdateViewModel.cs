using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.Navbar
{
    public class UpdateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Url { get; set; }

        [Required]
        public int RowNumber { get; set; }

        public bool IsFooter { get; set; }

        public bool IsHeader { get; set; }
    }
}
