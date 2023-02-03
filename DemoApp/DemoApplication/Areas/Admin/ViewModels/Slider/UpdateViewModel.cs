using DemoApplication.Attriubute;
using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.Slider
{
    public class UpdateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Offer { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string BackgroundImageUrl { get; set; }
        public IFormFile BgImage { get; set; }

        [Required]
        public string ButtonName { get; set; }

        [Required]
        public string BtnRedirectUrl { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
