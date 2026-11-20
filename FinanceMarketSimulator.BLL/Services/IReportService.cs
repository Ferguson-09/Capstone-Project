
using FinanceMarketSimulator.BLL.DTOs;

namespace FinanceMarketSimulator.BLL.Services
{
    public interface IReportService
    {
        Task<FinancialReportDto> GetLast30DaysReportAsync(string userId);

        Task<FinancialReportDto> GetLast90DaysReportAsync(string userId);

        Task<FinancialReportDto> GetCustomReportAsync(
            string userId,
            DateTime startDate,
            DateTime endDate);
    }
}
