using Atithi.Web.Models.DTO;
using Atithi.Web.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atithi.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            this._menuService = menuService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody] MenuDTO menuDto)
        {
            if (menuDto == null || string.IsNullOrWhiteSpace(menuDto.ItemName))
            {
                return BadRequest("Menu item name is required.");
            }
            try
            {
                bool result = await _menuService.AddMenuItem(menuDto);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, "An error occurred while saving to the database. Please try again.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }

            return Ok(true);
        }

        [HttpGet()]
        public async Task<IActionResult> GetMenu()
        {
            try
            {
                // Fetch all menu items and map them to MenuDTO objects
                var menuItems = await _menuService.GetAllMenu();

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
