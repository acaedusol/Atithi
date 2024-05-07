namespace Atithi.Web.Models.DTO
{
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();// Primary key
        public Guid OrderId { get; set; } // Foreign key referencing Order
        public Guid MenuItemId { get; set; } // Foreign key referencing Menu
        public int Quantity { get; set; } // Quantity of the ordered item
    }
}
