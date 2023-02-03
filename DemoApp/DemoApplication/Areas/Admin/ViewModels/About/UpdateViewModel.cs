using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.About
{
    public class UpdateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Content { get; set; }

    }
}