namespace DemoApplication.Areas.Admin.ViewModels.Plant.Add
{
    public class SubCategoryListItemViewModel
    {
        public int SubCategoryId { get; set; }

        public List<SubCategorItemViewModel> SubCategories { get; set; }


        public class SubCategorItemViewModel
        {
            public SubCategorItemViewModel(int id, string title)
            {
                Id = id;
                Title = title;
            }

            public int Id { get; set; }
            public string Title { get; set; }
        }
    }
}
