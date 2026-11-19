
using FinanceMarketSimulator.BLL.DTOs;
using FinanceMarketSimulator.DAL.Data;
using FinanceMarketSimulator.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceMarketSimulator.BLL.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly ApplicationDbContext _context;

        public BudgetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BudgetDto>> GetAllBudgetsAsync(string userId)
        {
            var budgets = await _context.Budgets
                .Include(b => b.Category)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            var result = new List<BudgetDto>();

            foreach (var budget in budgets)
            {
                result.Add(await MapBudgetAsync(budget));
            }

            return result;
        }

        public async Task<BudgetDto?> GetBudgetByIdAsync(
            int id,
            string userId)
        {
            var budget = await _context.Budgets
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b =>
                    b.Id == id && b.UserId == userId);

            if (budget == null)
            {
                return null;
            }

            return await MapBudgetAsync(budget);
        }

        public async Task<BudgetDto> CreateBudgetAsync(
            CreateBudgetDto dto,
            string userId)
        {
            if (dto.Amount <= 0)
            {
                throw new InvalidOperationException(
                    "Budget amount must be greater than zero.");
            }

            if (dto.EndDate < dto.StartDate)
            {
                throw new InvalidOperationException(
                    "End date must be after the start date.");
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c =>
                    c.Id == dto.CategoryId &&
                    c.UserId == userId);

            if (category == null)
            {
                throw new InvalidOperationException("Category not found.");
            }

            if (!category.Type.Equals(
                "Expense",
                StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "A budget must use an Expense category.");
            }

            var budget = new Budget
            {
                Name = dto.Name,
                Amount = dto.Amount,
                SpentAmount = 0,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                UserId = userId,
                CategoryId = dto.CategoryId
            };

            _context.Budgets.Add(budget);

            await _context.SaveChangesAsync();

            budget.Category = category;

            return await MapBudgetAsync(budget);
        }

        public async Task<bool> DeleteBudgetAsync(int id, string userId)
        {
            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b =>
                    b.Id == id && b.UserId == userId);

            if (budget == null)
            {
                return false;
            }

            _context.Budgets.Remove(budget);

            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<BudgetDto> MapBudgetAsync(Budget budget)
        {
            var spentAmount = await _context.Transactions
                .Where(t =>
                    t.UserId == budget.UserId &&
                    t.CategoryId == budget.CategoryId &&
                    t.Type == "Expense" &&
                    t.TransactionDate >= budget.StartDate &&
                    t.TransactionDate <= budget.EndDate)
                .SumAsync(t => (decimal?)t.Amount) ?? 0;

            return new BudgetDto
            {
                Id = budget.Id,
                Name = budget.Name,
                Amount = budget.Amount,
                SpentAmount = spentAmount,
                RemainingAmount = budget.Amount - spentAmount,
                IsExceeded = spentAmount > budget.Amount,
                StartDate = budget.StartDate,
                EndDate = budget.EndDate,
                CategoryId = budget.CategoryId,
                CategoryName = budget.Category?.Name ?? string.Empty
            };
        }
    }
}
