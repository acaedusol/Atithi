using Atithi.Web.Context;
using Atithi.Web.Models.Domain;
using Atithi.Web.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atithi.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly AtithiDbContext _atithiDbContext;
        public MenuController(AtithiDbContext atithiDbContext)
        {
            this._atithiDbContext = atithiDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody] MenuDTO menuDto)
        {
            if (menuDto == null || string.IsNullOrWhiteSpace(menuDto.ItemName))
            {
                return BadRequest("Menu item name is required.");
            }

            var menu = new Menu
            {
                MenuId = Guid.NewGuid(),
                ItemName = menuDto.ItemName,
                Price = menuDto.Price,
                Availability = menuDto.Availability,
                CategoryId = menuDto.CategoryId
            };
            try
            {
                this._atithiDbContext.Menus.Add(menu);
                await this._atithiDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, "An error occurred while saving to the database. Please try again.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }

            return Ok(); // Return the created menu DTO
        }

        [HttpGet()]
        public async Task<IActionResult> GetMenu()
        {
            try
            {
                // Fetch all menu items and map them to MenuDTO objects
                var menuItems = await this._atithiDbContext.Menus
                    .Select(menu => new MenuDTO
                    {
                        MenuId = menu.MenuId,
                        ItemName = menu.ItemName,
                        Price = menu.Price,
                        Availability = menu.Availability,
                        CategoryId = menu.CategoryId
                    })
                    .ToListAsync(); // Asynchronous database call

                if (menuItems == null || menuItems.Count == 0)
                {
                    // If no menu items are found, return 404 Not Found
                    return NotFound("No menu items found.");
                }

                // Return the list of menu items
                return Ok(menuItems);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, "An error occurred while fetching menu items. Please try again later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

    }

}
