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

}
