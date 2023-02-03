using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.Size
{
    public class AddViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
