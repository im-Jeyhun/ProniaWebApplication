namespace DemoApplication.Areas.Client.ViewModels.Home.Index
{
    public class FedBackListItemViewModel
    {
        public FedBackListItemViewModel(string profilPhotoName, string profilePhotoUrl, string fullName, string role, string content)
        {
            ProfilPhotoName = profilPhotoName;
            ProfilePhotoUrl = profilePhotoUrl;
            FullName = fullName;
            Role = role;
            Content = content;
        }

        public string ProfilPhotoName { get; set; }

        public string ProfilePhotoUrl { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Content { get; set; }
    }
}
