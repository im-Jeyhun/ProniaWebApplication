using DemoApplication.Database.Models.Enums;

namespace DemoApplication.Areas.Admin.ViewModels.Order
{
    public class OrderUpdateViewModel
    {
        public string OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }

    }
}
