using Atithi.Web.Models.DTO;
namespace Atithi.Web.Services.Interface
{
    public interface IMenuService
    {
        Task<MenuDTO> GetMenuById(Guid menuId); // Fetch a menu item by MenuId
        Task<bool> AddMenuItem(MenuDTO menu);
        Task<List<MenuDTO>> GetAllMenu();
        Task<decimal> CalculateItemTotalAsync(int quantity, Guid menuId);
    }

}
