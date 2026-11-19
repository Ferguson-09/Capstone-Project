
using FinanceMarketSimulator.BLL.DTOs;

namespace FinanceMarketSimulator.BLL.Services
{
    public interface IBudgetService
    {
        Task<List<BudgetDto>> GetAllBudgetsAsync(string userId);

        Task<BudgetDto?> GetBudgetByIdAsync(
            int id,
            string userId);

        Task<BudgetDto> CreateBudgetAsync(
            CreateBudgetDto dto,
            string userId);

        Task<bool> DeleteBudgetAsync(int id, string userId);
    }
}
