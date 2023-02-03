namespace DemoApplication.Areas.Admin.ViewModels.Contact
{
    public class ListItemViewModel
    {
        public ListItemViewModel(int id, string name, string lastName, string email, string phone, string message)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Message = message;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }

    }
}
