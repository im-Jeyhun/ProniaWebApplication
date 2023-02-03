using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class Navbar : BaseNavbarAndSubNavbar, IAuditable 
    {

        public bool IsFooter { get; set; }

        public bool IsHeader { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<SubNavbar>? SubNavbars { get; set; }
    }
}
