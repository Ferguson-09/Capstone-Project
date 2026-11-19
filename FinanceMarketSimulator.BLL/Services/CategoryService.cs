
using FinanceMarketSimulator.BLL.DTOs;
using FinanceMarketSimulator.DAL.Data;
using FinanceMarketSimulator.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceMarketSimulator.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync(string userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type
                })
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id, string userId)
        {
            return await _context.Categories
                .Where(c => c.Id == id && c.UserId == userId)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CategoryDto> CreateCategoryAsync(
            CreateCategoryDto dto,
            string userId)
        {
            var category = new Category
            {
                Name = dto.Name,
                Type = dto.Type,
                UserId = userId
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Type = category.Type
            };
        }

        public async Task<bool> DeleteCategoryAsync(int id, string userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (category == null)
            {
                return false;
            }

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
