
using FinanceMarketSimulator.BLL.DTOs;
using FinanceMarketSimulator.DAL.Data;
using FinanceMarketSimulator.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceMarketSimulator.BLL.Services
{
    public class FinancialAccountService : IFinancialAccountService
    {
        private readonly ApplicationDbContext _context;

        public FinancialAccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FinancialAccountDto>> GetAllAccountsAsync(string userId)
        {
            return await _context.FinancialAccounts
                .Where(a => a.UserId == userId)
                .Select(a => new FinancialAccountDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    AccountType = a.AccountType,
                    Balance = a.Balance
                })
                .ToListAsync();
        }

        public async Task<FinancialAccountDto?> GetAccountByIdAsync(int id, string userId)
        {
            return await _context.FinancialAccounts
                .Where(a => a.Id == id && a.UserId == userId)
                .Select(a => new FinancialAccountDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    AccountType = a.AccountType,
                    Balance = a.Balance
                })
                .FirstOrDefaultAsync();
        }

        public async Task<FinancialAccountDto> CreateAccountAsync(
            CreateFinancialAccountDto dto,
            string userId)
        {
            var account = new FinancialAccount
            {
                Name = dto.Name,
                AccountType = dto.AccountType,
                Balance = dto.Balance,
                UserId = userId
            };

            _context.FinancialAccounts.Add(account);

            await _context.SaveChangesAsync();

            return new FinancialAccountDto
            {
                Id = account.Id,
                Name = account.Name,
                AccountType = account.AccountType,
                Balance = account.Balance
            };
        }

        public async Task<bool> DeleteAccountAsync(int id, string userId)
        {
            var account = await _context.FinancialAccounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (account == null)
            {
                return false;
            }

            if (account.Transactions.Any())
            {
                return false;
            }

            _context.FinancialAccounts.Remove(account);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
