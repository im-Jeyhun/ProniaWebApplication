namespace DemoApplication.Areas.Client.ViewModels.Home.Index
{
    public class SliderListItemViewModel
    {
        public SliderListItemViewModel(string offer, string title, string content, string bgImageName, string bgImageUrl, string buttonName, string btnRedirectUrl, int order)
        {
            Offer = offer;
            Title = title;
            Content = content;
            BgImageName = bgImageName;
            BgImageUrl = bgImageUrl;
            ButtonName = buttonName;
            BtnRedirectUrl = btnRedirectUrl;
            Order = order;
        }

        public string Offer { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string BgImageName { get; set; }
        public string BgImageUrl { get; set; }
        public string ButtonName { get; set; }
        public string BtnRedirectUrl { get; set; }
        public int Order { get; set; }
    }
}
