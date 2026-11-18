
using System.ComponentModel.DataAnnotations;

namespace FinanceMarketSimulator.DAL.Entities
{
    public class FinancialAccount
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string AccountType { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
            = new List<Transaction>();
    }
}
