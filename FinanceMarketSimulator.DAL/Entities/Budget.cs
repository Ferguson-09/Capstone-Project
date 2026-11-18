
using System.ComponentModel.DataAnnotations;

namespace FinanceMarketSimulator.DAL.Entities
{
    public class Budget
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public decimal SpentAmount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
