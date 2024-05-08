namespace Atithi.Web.Models.DTO
{
    public class CategoryDTO
    {
        public Guid CategoryId { get; set; } = Guid.NewGuid(); // Primary key
        public string CategoryName { get; set; } // Name of the category
    }

    public class CategoryMenuDTO
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<MenuDTO> Items { get; set; } = new List<MenuDTO>(); // List of MenuDTO
    }
}
