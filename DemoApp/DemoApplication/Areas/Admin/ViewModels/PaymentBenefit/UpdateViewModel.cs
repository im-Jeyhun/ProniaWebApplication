using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.PaymentBenefit
{
    public class UpdateViewModel
    {
        [Required]
        public string? Name { get; set; }
        public string? ImageName { get; set; }
        public string? ImageUrl { get; set; }

        [Required]
        public string? Content { get; set; }
        public IFormFile? PbImage { get; set; }

    }
}