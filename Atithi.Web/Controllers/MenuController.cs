using Atithi.Web.Context;
using Atithi.Web.Models.Domain;
using Atithi.Web.Models.DTO;
using Atithi.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Atithi.Web.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly AtithiDbContext _atithiDbContext;
        public MenuController(AtithiDbContext atithiDbContext)
        {
            this._atithiDbContext = atithiDbContext;
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] string categoryName)
        {
            if (categoryName == null || string.IsNullOrWhiteSpace(categoryName))
            {
                return BadRequest("Category name is required.");
            }

            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = categoryName.ToLower()
            };

            try
            {
                this._atithiDbContext.Categories.Add(category);
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

            return Ok(category); // Return the created category
        }

        [HttpGet("categories/{id:guid}")] // Fetch category by its GUID
        public async Task<IActionResult> GetCategory(Guid id)
        {
            try
            {
                // Fetch the category with the given ID
                var category = await this._atithiDbContext.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == id);

                if (category == null)
                {
                    // If no category is found, return a 404 Not Found response
                    return NotFound($"Category with ID {id} not found.");
                }

                CategoryDTO result = new CategoryDTO
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName.ToTitleCase(),
                };

                // Return the category
                return Ok(result);
            }
            catch (DbUpdateException dbEx)
            {
                // Log and handle database-related exceptions
                Console.WriteLine($"Database error: {dbEx.Message}");
                return StatusCode(500, "An error occurred while fetching the category. Please try again later.");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("categories")] // Endpoint for fetching all categories
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                // Fetch all categories from the database
                var categories = await this._atithiDbContext.Categories
                    .Select(category => new CategoryDTO
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName.ToTitleCase(),
                    })
                    .ToListAsync();

                if (categories == null || !categories.Any())
                {
                    // If no categories are found, return an appropriate response
                    return NotFound("No categories found.");
                }

                // Return the list of categories
                return Ok(categories);
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-related exceptions
                Console.WriteLine($"Database error: {dbEx.Message}");
                return StatusCode(500, "An error occurred while fetching categories. Please try again later.");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPost("menu")]
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

            return Ok(menuDto); // Return the created menu DTO
        }

        [HttpGet("menu")]
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
