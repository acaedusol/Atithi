using Atithi.Web.Context;
using Atithi.Web.Models.Domain;
using Atithi.Web.Models.DTO;
using Atithi.Web.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Atithi.Web.Services
{
    public class MenuService : IMenuService
    {
        private readonly AtithiDbContext _atithiDbContext;
        public MenuService(AtithiDbContext atithiDbContext)
        {
            _atithiDbContext = atithiDbContext;
        }

        public async Task<MenuDTO> GetMenuById(Guid menuId)
        {
            try
            {
                // Fetch the menu item with the specified MenuId
                var menu = await _atithiDbContext.Menus
                    .AsNoTracking()
                    .FirstOrDefaultAsync(menu => menu.MenuId == menuId); // Return the first match or null

                if (menu == null)
                {
                    return null;
                }

                var result = new MenuDTO
                {
                    MenuId = menuId,
                    Availability = menu.Availability,
                    CategoryId = menu.CategoryId,
                    ImagePath = menu.ImagePath,
                    ItemName = menu.ItemName,
                    Price = menu.Price,

                };
                return result; // Return the found menu item or null
            }
            catch (Exception ex)
            {
                throw; // Re-throw the exception or handle it appropriately
            }
        }

        public async Task<bool> AddMenuItem(MenuDTO menu)
        {
            var menuItem = new Menu
            {
                MenuId = Guid.NewGuid(),
                ItemName = menu.ItemName,
                Price = menu.Price,
                Availability = menu.Availability,
                CategoryId = menu.CategoryId,
                ImagePath = menu.ImagePath,
            };
            try
            {
                this._atithiDbContext.Menus.Add(menuItem);
                await _atithiDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MenuDTO>> GetAllMenu()
        {
            return await this._atithiDbContext.Menus
                    .Select(menu => new MenuDTO
                    {
                        MenuId = menu.MenuId,
                        ItemName = menu.ItemName,
                        Price = menu.Price,
                        Availability = menu.Availability,
                        CategoryId = menu.CategoryId,
                        ImagePath = menu.ImagePath,
                    })
                    .ToListAsync();
        }

        public async Task<decimal> CalculateItemTotalAsync(int quantity, Guid menuId)
        {
            // Assuming you have a method to get the menu item price
            decimal itemPrice = (await GetMenuById(menuId)).Price;
            return itemPrice * quantity; // Calculate total price for this order item
        }
    }
}
