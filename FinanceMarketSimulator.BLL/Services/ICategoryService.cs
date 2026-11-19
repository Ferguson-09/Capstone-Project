
using FinanceMarketSimulator.BLL.DTOs;

namespace FinanceMarketSimulator.BLL.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesAsync(string userId);

        Task<CategoryDto?> GetCategoryByIdAsync(int id, string userId);

        Task<CategoryDto> CreateCategoryAsync(
            CreateCategoryDto dto,
            string userId);

        Task<bool> DeleteCategoryAsync(int id, string userId);
    }
}
