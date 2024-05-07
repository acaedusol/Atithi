namespace Atithi.Web.Models.Domain
{
    using System;
    using System.Collections.Generic;

    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid(); // Primary key for the order
        public int RoomId { get; set; } // The room from which the order is placed
        public DateTime OrderDate { get; set; } // The date and time when the order is placed
        public bool IsDelivered { get; set; } // Indicates if the order has been delivered

        // Navigation property to the list of order items
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public decimal CalculateTotalPrice()
        {
            decimal total = 0;
            foreach (var orderItem in OrderItems)
            {
                total += orderItem.CalculateItemTotal(); // Sum the total for all order items
            }
            return total; // Return the total price for this order
        }

        // Business rules and domain logic for the order
        public void MarkAsDelivered()
        {
            // Business logic to mark the order as delivered
            if (!IsDelivered)
            {
                IsDelivered = true; // Mark as delivered
            }
        }
    }
}
