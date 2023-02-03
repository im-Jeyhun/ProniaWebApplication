using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.About
{
    public class AddViewModel
    {
      

        [Required]
        public string? Content { get; set; }
    }
}
