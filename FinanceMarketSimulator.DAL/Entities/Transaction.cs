
using System.ComponentModel.DataAnnotations;

namespace FinanceMarketSimulator.DAL.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public int FinancialAccountId { get; set; }

        public FinancialAccount? FinancialAccount { get; set; }

        public int? TransferToAccountId { get; set; }

        public FinancialAccount? TransferToAccount { get; set; }
    }
}
