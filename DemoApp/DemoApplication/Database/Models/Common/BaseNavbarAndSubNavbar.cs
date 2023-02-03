namespace DemoApplication.Database.Models.Common
{
    public abstract class BaseNavbarAndSubNavbar : BaseEntity<int>
    {

        public string Name { get; set; }

        public string Url { get; set; }

        public int RowNumber { get; set; }
    }
}
