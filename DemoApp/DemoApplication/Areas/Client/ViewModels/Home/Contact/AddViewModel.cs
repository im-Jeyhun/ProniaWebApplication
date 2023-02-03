using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Client.ViewModels.Home.Contact
{
    public class AddViewModel
    {
        public AddViewModel()
        {

        }
        public AddViewModel(string? name, string? lastName, string? email, string? message, string? phoneNumber)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            Message = message;
            PhoneNumber = phoneNumber;
        }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        public string? Message { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

    }
}
