using DemoApplication.Database.Models.Common;

namespace DemoApplication.Database.Models
{
    public class SubNavbar : BaseNavbarAndSubNavbar , IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int NavbarId { get; set; }
        public Navbar? Navbar { get; set; }

    }
}
