using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.PaymentBenefit
{
    public class AddViewModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public IFormFile? PbImage { get; set; }

        [Required]
        public string? Content { get; set; }
    }
}
