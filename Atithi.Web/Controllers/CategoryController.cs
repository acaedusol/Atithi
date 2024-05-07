using Atithi.Web.Context;
using Atithi.Web.Models.Domain;
using Atithi.Web.Models.DTO;
using Atithi.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atithi.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController: ControllerBase
    {
        private readonly AtithiDbContext _atithiDbContext;
        public CategoryController(AtithiDbContext atithiDbContext)
        {
            this._atithiDbContext = atithiDbContext;
        }

        [HttpPost("{categoryName}")]
        public async Task<IActionResult> CreateCategory(string categoryName)
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

            return Ok(); // Return the created category
        }

        [HttpGet("{categoryId:guid}")] // Fetch category by its GUID
        public async Task<IActionResult> GetCategory(Guid categoryId)
        {
            try
            {
                // Fetch the category with the given ID
                var category = await this._atithiDbContext.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

                if (category == null)
                {
                    // If no category is found, return a 404 Not Found response
                    return NotFound($"Category with ID {categoryId} not found.");
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
                return StatusCode(500, "An error occurred while fetching the category. Please try again later.");
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
                return StatusCode(500, "An error occurred while fetching categories. Please try again later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
