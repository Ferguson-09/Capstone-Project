
using System.ComponentModel.DataAnnotations;

namespace FinanceMarketSimulator.DAL.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
            = new List<Transaction>();

        public ICollection<Budget> Budgets { get; set; }
            = new List<Budget>();
    }
}
