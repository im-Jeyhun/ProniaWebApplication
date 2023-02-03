using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.Category
{
    public class AddViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }
    }
}
