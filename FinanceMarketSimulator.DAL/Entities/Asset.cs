namespace FinanceMarketSimulator.DAL.Entities
{
    public enum AssetType
    {
        Stock,
        Cryptocurrency
    }

    public class Asset
    {
        public int Id { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public AssetType Type { get; set; }

        public decimal CurrentPrice { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool IsActive { get; set; }
    }
}