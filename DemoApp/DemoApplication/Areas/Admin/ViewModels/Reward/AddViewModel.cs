using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.Reward
{
    public class AddViewModel
    {
        [Required]
        public IFormFile RewardImage { get; set; }
    }
}
