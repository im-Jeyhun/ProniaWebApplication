using DemoApplication.Database.Models.Common;
using DemoApplication.Database.Models.Enums;

namespace DemoApplication.Database.Models
{
    public class UserActivation : BaseEntity<int>
    {
        public string? ActivationUrl { get; set; }
        public string? ActivationToken { get; set; }
        public DateTime TokenExpireDate { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public ActivationTokenTypes Type { get; set; }
    }

}
