using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.Color
{
    public class UpdateVeiwModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
