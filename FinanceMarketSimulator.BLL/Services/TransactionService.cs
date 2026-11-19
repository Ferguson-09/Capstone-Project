
using FinanceMarketSimulator.BLL.DTOs;
using FinanceMarketSimulator.DAL.Data;
using FinanceMarketSimulator.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceMarketSimulator.BLL.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TransactionDto>> GetAllTransactionsAsync(string userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Type = t.Type,
                    Description = t.Description,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category!.Name,
                    FinancialAccountId = t.FinancialAccountId,
                    AccountName = t.FinancialAccount!.Name,
                    TransferToAccountId = t.TransferToAccountId,
                    TransferToAccountName = t.TransferToAccount != null
                        ? t.TransferToAccount.Name
                        : null
                })
                .ToListAsync();
        }

        public async Task<TransactionDto?> GetTransactionByIdAsync(
            int id,
            string userId)
        {
            return await _context.Transactions
                .Where(t => t.Id == id && t.UserId == userId)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Type = t.Type,
                    Description = t.Description,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category!.Name,
                    FinancialAccountId = t.FinancialAccountId,
                    AccountName = t.FinancialAccount!.Name,
                    TransferToAccountId = t.TransferToAccountId,
                    TransferToAccountName = t.TransferToAccount != null
                        ? t.TransferToAccount.Name
                        : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<TransactionDto> CreateTransactionAsync(
            CreateTransactionDto dto,
            string userId)
        {
            if (dto.Amount <= 0)
            {
                throw new InvalidOperationException("Amount must be greater than zero.");
            }

            var type = dto.Type.Trim().ToLower();

            if (type != "income" && type != "expense" && type != "transfer")
            {
                throw new InvalidOperationException(
                    "Transaction type must be Income, Expense or Transfer.");
            }

            var account = await _context.FinancialAccounts
                .FirstOrDefaultAsync(a =>
                    a.Id == dto.FinancialAccountId &&
                    a.UserId == userId);

            if (account == null)
            {
                throw new InvalidOperationException("Financial account not found.");
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c =>
                    c.Id == dto.CategoryId &&
                    c.UserId == userId);

            if (category == null)
            {
                throw new InvalidOperationException("Category not found.");
            }

            if (type != "transfer" &&
                !category.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Category type must match the transaction type.");
            }

            FinancialAccount? destinationAccount = null;

            if (type == "transfer")
            {
                if (dto.TransferToAccountId == null ||
                    dto.TransferToAccountId == dto.FinancialAccountId)
                {
                    throw new InvalidOperationException(
                        "Choose a different destination account for the transfer.");
                }

                destinationAccount = await _context.FinancialAccounts
                    .FirstOrDefaultAsync(a =>
                        a.Id == dto.TransferToAccountId &&
                        a.UserId == userId);

                if (destinationAccount == null)
                {
                    throw new InvalidOperationException(
                        "Destination account not found.");
                }

                if (account.Balance < dto.Amount)
                {
                    throw new InvalidOperationException(
                        "Insufficient funds in the source account.");
                }

                account.Balance -= dto.Amount;
                destinationAccount.Balance += dto.Amount;
            }
            else if (type == "income")
            {
                account.Balance += dto.Amount;
            }
            else
            {
                if (account.Balance < dto.Amount)
                {
                    throw new InvalidOperationException(
                        "Insufficient funds in the account.");
                }

                account.Balance -= dto.Amount;
            }

            var transaction = new Transaction
            {
                Type = type == "income" ? "Income" :
                       type == "expense" ? "Expense" : "Transfer",
                Description = dto.Description,
                Amount = dto.Amount,
                TransactionDate = dto.TransactionDate,
                UserId = userId,
                CategoryId = dto.CategoryId,
                FinancialAccountId = dto.FinancialAccountId,
                TransferToAccountId = dto.TransferToAccountId
            };

            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();

            return new TransactionDto
            {
                Id = transaction.Id,
                Type = transaction.Type,
                Description = transaction.Description,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                CategoryId = category.Id,
                CategoryName = category.Name,
                FinancialAccountId = account.Id,
                AccountName = account.Name,
                TransferToAccountId = destinationAccount?.Id,
                TransferToAccountName = destinationAccount?.Name
            };
        }

        public async Task<bool> DeleteTransactionAsync(int id, string userId)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t =>
                    t.Id == id && t.UserId == userId);

            if (transaction == null)
            {
                return false;
            }

            var account = await _context.FinancialAccounts
                .FirstOrDefaultAsync(a =>
                    a.Id == transaction.FinancialAccountId &&
                    a.UserId == userId);

            if (account == null)
            {
                throw new InvalidOperationException("Financial account not found.");
            }

            if (transaction.Type.Equals("Income", StringComparison.OrdinalIgnoreCase))
            {
                account.Balance -= transaction.Amount;
            }
            else if (transaction.Type.Equals("Expense", StringComparison.OrdinalIgnoreCase))
            {
                account.Balance += transaction.Amount;
            }
            else if (transaction.Type.Equals("Transfer", StringComparison.OrdinalIgnoreCase))
            {
                var destinationAccount = await _context.FinancialAccounts
                    .FirstOrDefaultAsync(a =>
                        a.Id == transaction.TransferToAccountId &&
                        a.UserId == userId);

                if (destinationAccount == null)
                {
                    throw new InvalidOperationException(
                        "Destination account not found.");
                }

                account.Balance += transaction.Amount;
                destinationAccount.Balance -= transaction.Amount;
            }

            _context.Transactions.Remove(transaction);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
