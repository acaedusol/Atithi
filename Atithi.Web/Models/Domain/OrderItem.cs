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

        public decimal CalculateItemTotal()
        {
            // Assuming you have a method to get the menu item price
            decimal itemPrice = GetMenuItemPrice(MenuItemId);
            return itemPrice * Quantity; // Calculate total price for this order item
        }

        private decimal GetMenuItemPrice(Guid menuItemId)
        {
            return 10;
        }
    }

}
