
namespace FinanceMarketSimulator.BLL.DTOs
{
    public class CreateTransactionDto
    {
        public string Type { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public int CategoryId { get; set; }

        public int FinancialAccountId { get; set; }

        public int? TransferToAccountId { get; set; }
    }
}
