using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.Color
{
    public class AddViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
