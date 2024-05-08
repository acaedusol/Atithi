using Atithi.Web.Context;
using Atithi.Web.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atithi.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        [HttpPost("{categoryName}")]
        public async Task<IActionResult> CreateCategory(string categoryName)
        {
            if (categoryName == null || string.IsNullOrWhiteSpace(categoryName))
            {
                return BadRequest("Category name is required.");
            }

            try
            {
                var result = await _categoryService.AddCategoryItem(categoryName);
                return Ok(result);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, "An error occurred while saving to the database. Please try again.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }

        }

        [HttpGet("{categoryId:guid}")] // Fetch category by its GUID
        public async Task<IActionResult> GetCategory(Guid categoryId)
        {
            try
            {
                // Fetch the category with the given ID
                var result = await _categoryService.GetCategoryById(categoryId);

                if (result == null)
                {
                    // If no category is found, return a 404 Not Found response
                    return NotFound($"Category with ID {categoryId} not found.");
                }
                // Return the category
                return Ok(result);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, "An error occurred while fetching the category. Please try again later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("categorymenu")] // Endpoint for fetching all categories
        public async Task<IActionResult> GetAllCategoryMenu()
        {
            try
            {
                // Fetch all categories from the database
                var categoryMenu = await this._categoryService.GetMenuByCategoryAsync();

                if (categoryMenu == null || !categoryMenu.Any())
                {
                    // If no categories are found, return an appropriate response
                    return NotFound("No Data found.");
                }

                // Return the list of categories
                return Ok(categoryMenu);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, "An error occurred while fetching categories. Please try again later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet] // Endpoint for fetching all categories
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                // Fetch all categories from the database
                var categories = await this._categoryService.GetAllCategory();

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
                return StatusCode(500, "An error occurred while fetching categories. Please try again later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
