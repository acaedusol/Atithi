using Atithi.Web.Models.DTO;

namespace Atithi.Web.Services.Interface
{
    public interface ICategoryService
    {
        Task<bool> AddCategoryItem(string categoryName);
        Task<CategoryDTO> GetCategoryById(Guid categoryId);
        Task<List<CategoryDTO>> GetAllCategory();
        Task<List<CategoryMenuDTO>> GetMenuByCategoryAsync();
    }
}
