using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.SubCategory
{
    public class AddViewModel
    {
        [Required]
        public string? Title { get; set; }

        public int CategoryId { get; set; }
        public List<CategoryViewModel>? Categories { get; set; }

        public class CategoryViewModel
        {
            public CategoryViewModel(int id, string title)
            {
                Id = id;
                Title = title;
            }

            public int Id { get; set; }

            public string Title { get; set; }
        }
    }
}
