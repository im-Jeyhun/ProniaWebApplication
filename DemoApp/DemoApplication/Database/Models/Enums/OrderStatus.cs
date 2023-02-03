using DemoApplication.Database.Models;

namespace DemoApplication.Database.Models.Enums
{
    public enum OrderStatus
    {
        Created = 1,
        Confirmed = 2,
        Rejected = 3,
        Sended = 4,
        Completed = 5,
    }
    public static class StatusStatusCode
    {
        public static string GetStatusCode(this OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Created:
                    return "Created";
                case OrderStatus.Confirmed:
                    return "Confirmed";
                case OrderStatus.Rejected:
                    return "Rejected";
                case OrderStatus.Sended:
                    return "Sended";
                case OrderStatus.Completed:
                    return "Completed";
                default:
                    throw new Exception("This status code not found");

            }
        }
    }

    public static class StatusMessage
    {
        public static string GetStatusMessage(this OrderStatus status, User user, string orderNumber)
        {
            switch (status)
            {
                case OrderStatus.Created:
                    return $"Hörmətli {user.FirstName} {user.LastName}, sizin {orderNumber} yaradildi.";
                case OrderStatus.Confirmed:
                    return $"Hörmətli {user.FirstName} {user.LastName}, sizin {orderNumber} tesdiqlendi.";
                case OrderStatus.Rejected:
                    return $"Hörmətli {user.FirstName} {user.LastName}, sizin {orderNumber} tesdiqlenmedi.";
                case OrderStatus.Sended:
                    return $"Hörmətli {user.FirstName} {user.LastName}, sizin {orderNumber} göndərildi, kuryer sizinlə əlaqə saxlayacaq..";
                case OrderStatus.Completed:
                    return $"Hörmətli {user.FirstName} {user.LastName}, sizin {orderNumber} kuryer tərəfindən təhvil verildi..";
                default:
                    throw new Exception("This status code not found");

            }
        }
    }
}