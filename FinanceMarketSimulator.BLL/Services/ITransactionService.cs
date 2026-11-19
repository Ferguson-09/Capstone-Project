
using FinanceMarketSimulator.BLL.DTOs;

namespace FinanceMarketSimulator.BLL.Services
{
    public interface ITransactionService
    {
        Task<List<TransactionDto>> GetAllTransactionsAsync(string userId);

        Task<TransactionDto?> GetTransactionByIdAsync(
            int id,
            string userId);

        Task<TransactionDto> CreateTransactionAsync(
            CreateTransactionDto dto,
            string userId);

        Task<bool> DeleteTransactionAsync(int id, string userId);
    }
}
