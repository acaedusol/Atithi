namespace Atithi.Web.Models.Domain
{
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Primary key
        public Guid OrderId { get; set; } // Foreign key referencing Order
        public Guid MenuItemId { get; set; } // Foreign key referencing Menu
        public int Quantity { get; set; } // Quantity of the ordered item

        // Navigation properties
        public Order Order { get; set; } // The order to which this item belongs
        public Menu Menu { get; set; } // The menu item being ordered

    }

}
