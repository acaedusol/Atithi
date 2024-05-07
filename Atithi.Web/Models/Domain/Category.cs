using System.ComponentModel.DataAnnotations;

namespace Atithi.Web.Models.Domain
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; } = Guid.NewGuid(); // Primary key
        public string CategoryName { get; set; } // Name of the category

        // Navigation property to the Menu items belonging to this category
        public List<Menu> Menus { get; set; } = new List<Menu>();
    }

}
