using DemoApplication.Contracts.User;

namespace DemoApplication.Areas.Client.ViewModels.Home.Index
{
    public class IndexViewModel
    {
        public List<PlantListItemViewModel>? Plants { get; set; }
        public List<SliderListItemViewModel>? Sliders { get; set; }

        public List<FedBackListItemViewModel>? FeedBacks { get; set; }
        public List<RewardItemViewModel>? Reward { get; set; }

        public class RewardItemViewModel
        {
            public RewardItemViewModel(string imageName, string imageUrl)
            {
                ImageName = imageName;
                ImageUrl = imageUrl;
            }

            public string ImageName { get; set; }
            public string ImageUrl { get; set; }
        }
        public class PaymentBenefitsViewModel
        {

            public string? Name { get; set; }
            public string? ImageName { get; set; }
            public string? ImageUrl { get; set; }
            public string? Content { get; set; }
            public PaymentBenefitsViewModel(string? name, string? imageName, string? imageUrl, string? content)
            {
                Name = name;
                ImageName = imageName;
                ImageUrl = imageUrl;
                Content = content;
            }
        }
    }
}
