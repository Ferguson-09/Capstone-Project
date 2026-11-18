
namespace FinanceMarketSimulator.BLL.DTOs
{
    public class CreateFinancialAccountDto
    {
        public string Name { get; set; } = string.Empty;

        public string AccountType { get; set; } = string.Empty;

        public decimal Balance { get; set; }
    }
}
