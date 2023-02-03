using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.Size
{
    public class UpdateVeiwModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
