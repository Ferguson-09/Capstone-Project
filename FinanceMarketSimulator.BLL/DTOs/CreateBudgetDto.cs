
namespace FinanceMarketSimulator.BLL.DTOs
{
    public class CreateBudgetDto
    {
        public string Name { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CategoryId { get; set; }
    }
}
