
namespace FinanceMarketSimulator.BLL.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public int FinancialAccountId { get; set; }

        public string AccountName { get; set; } = string.Empty;

        public int? TransferToAccountId { get; set; }

        public string? TransferToAccountName { get; set; }
    }
}
