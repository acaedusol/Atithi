namespace Atithi.Web.Models.DTO
{
    public class MenuDTO
    {
        public Guid MenuId { get; set; } = Guid.NewGuid(); // Primary key
        public string ItemName { get; set; } // Name of the menu item
        public decimal Price { get; set; } // Price of the menu item
        public bool Availability { get; set; } // Is the menu item available?
        public string ImagePath { get; set; } // ImagePath in Website
        public Guid CategoryId { get; set; } // Foreign key referencing Category
    }

}
