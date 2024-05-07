namespace Atithi.Web.Models.DTO
{
    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid(); // Primary key for the order
        public int RoomId { get; set; } // The room from which the order is placed
        public DateTime OrderDate { get; set; } // The date and time when the order is placed
        public bool IsDelivered { get; set; } // Indicates if the order has been delivered
    }
}
