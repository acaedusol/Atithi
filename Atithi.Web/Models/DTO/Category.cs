namespace Atithi.Web.Models.DTO
{
    public class CategoryDTO
    {
        public Guid CategoryId { get; set; } = Guid.NewGuid(); // Primary key
        public string CategoryName { get; set; } // Name of the category
    }

}
