using System.ComponentModel.DataAnnotations;

namespace Atithi.Web.Models.Domain
{
    public class Menu
    {
        [Key]
        public Guid MenuId { get; set; } = Guid.NewGuid(); // Primary key
        public string ItemName { get; set; } // Name of the menu item
        public decimal Price { get; set; } // Price of the menu item
        public bool Availability { get; set; } // Is the menu item available?

        public string ImagePath { get; set; } // ImagePath in Website

        // Foreign key to link to the Category
        public Guid CategoryId { get; set; } // Foreign key referencing Category

        // Navigation property to the Category this menu item belongs to
        public Category Category { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

}
