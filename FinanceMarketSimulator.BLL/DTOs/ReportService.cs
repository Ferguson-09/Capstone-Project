
using FinanceMarketSimulator.BLL.DTOs;
using FinanceMarketSimulator.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceMarketSimulator.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FinancialReportDto> GetLast30DaysReportAsync(
            string userId)
        {
            DateTime endDate = DateTime.Today;
            DateTime startDate = endDate.AddDays(-29);

            return await GetCustomReportAsync(
                userId, startDate, endDate);
        }

        public async Task<FinancialReportDto> GetLast90DaysReportAsync(
            string userId)
        {
            DateTime endDate = DateTime.Today;
            DateTime startDate = endDate.AddDays(-89);

            return await GetCustomReportAsync(
                userId, startDate, endDate);
        }

        public async Task<FinancialReportDto> GetCustomReportAsync(
            string userId,
            DateTime startDate,
            DateTime endDate)
        {
            if (endDate.Date < startDate.Date)
            {
                throw new InvalidOperationException(
                    "End date cannot be earlier than start date.");
            }

            DateTime exclusiveEndDate = endDate.Date.AddDays(1);

            var transactions = await _context.Transactions
                .Where(t =>
                    t.UserId == userId &&
                    t.TransactionDate >= startDate.Date &&
                    t.TransactionDate < exclusiveEndDate &&
                    (t.Type == "Income" || t.Type == "Expense"))
                .ToListAsync();

            decimal totalIncome = transactions
                .Where(t => t.Type == "Income")
                .Sum(t => t.Amount);

            decimal totalExpenses = transactions
                .Where(t => t.Type == "Expense")
                .Sum(t => t.Amount);

            return new FinancialReportDto
            {
                StartDate = startDate.Date,
                EndDate = endDate.Date,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                Balance = totalIncome - totalExpenses
            };
        }
    }
}
