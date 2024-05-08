namespace Atithi.Web.Models.DTO
{
    public class OrderDetailsDTO
    {
        public int RoomId { get; set; } // Room ID for the order
        public List<OrderItemDTO> OrderItems { get; set; } // List of order items

        public OrderDetailsDTO()
        {
            OrderItems = new List<OrderItemDTO>();
        }
    }

    public class OrderItemDTO
    {
        public Guid MenuItemId { get; set; } // ID of the menu item
        public int Quantity { get; set; } // Quantity of the item
    }

    public class OrderDeliveryDTO
    {
        public Guid OrderId { get; set; }
        public int RoomId { get; set;}
    }

    public class OrderFetchDetailsDTO
    {
        public Guid OrderId { get; set; } // Unique identifier for the order
        public int RoomId { get; set; } // Room ID where the order was placed
        public DateTime OrderDate { get; set; } // Date and time the order was placed
        public bool IsDelivered { get; set; } // Indicates if the order has been delivered

        // List of ordered items, including item details like name, quantity, and price
        public List<TotalOrderItemDTO> OrderItems { get; set; } = new List<TotalOrderItemDTO>();

        // Total price of the order, calculated from the order items
        public decimal TotalPrice { get; set; }
    }

    public class TotalOrderItemDTO
    {
        public Guid MenuId { get; set; } // Menu ID of the ordered item
        public string ItemName { get; set; } // Name of the menu item
        public int Quantity { get; set; } // Quantity of the ordered item
        public decimal PriceOfEach { get; set; }
        public decimal TotalPrice { get; set; } // Price of the menu item
    }

}
