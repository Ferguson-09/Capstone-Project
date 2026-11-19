
using FinanceMarketSimulator.BLL.DTOs;

namespace FinanceMarketSimulator.BLL.Services
{
    public interface IFinancialAccountService
    {
        Task<List<FinancialAccountDto>> GetAllAccountsAsync(string userId);

        Task<FinancialAccountDto?> GetAccountByIdAsync(
            int id,
            string userId);

        Task<FinancialAccountDto> CreateAccountAsync(
            CreateFinancialAccountDto dto,
            string userId);

        Task<bool> DeleteAccountAsync(int id, string userId);
    }
}
