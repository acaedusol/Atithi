using Atithi.Web.Context;
using Atithi.Web.Models.Domain;
using Atithi.Web.Models.DTO;
using Atithi.Web.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Atithi.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AtithiDbContext _atithiDbContext;
        public CategoryService(AtithiDbContext atithiDbContext)
        {
            _atithiDbContext = atithiDbContext;
        }
        public async Task<bool> AddCategoryItem(string categoryName)
        {
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
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }

        public async Task<List<CategoryDTO>> GetAllCategory()
        {
            try
            {
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
                    return null;
                }
                return categories;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CategoryDTO> GetCategoryById(Guid categoryId)
        {
            try
            {
                var category = await this._atithiDbContext.Categories
                        .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

                if (category == null)
                {
                    return null;
                }

                CategoryDTO result = new CategoryDTO
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName.ToTitleCase(),
                };
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CategoryMenuDTO>> GetMenuByCategoryAsync()
        {
            // Fetch all Menu items with their associated Category
            var menuItems = await _atithiDbContext.Menus
                .Include(m => m.Category) // Include the Category navigation property
                .ToListAsync();

            // Group Menu items by CategoryName
            var groupedByCategory = menuItems
                .GroupBy(m => new { m.Category.CategoryId, m.Category.CategoryName })
                .Select(g => new CategoryMenuDTO
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName.ToTitleCase(),
                    Items = g.Select(m => new MenuDTO
                    {
                        MenuId = m.MenuId,
                        ItemName = m.ItemName,
                        Price = m.Price,
                        Availability = m.Availability,
                        ImagePath = m.ImagePath,
                        CategoryId = m.CategoryId,
                    }).ToList() // List of MenuDTO
                })
                .ToList();

            return groupedByCategory; // Return the structured data
        }
    }
}