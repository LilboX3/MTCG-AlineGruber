namespace MTCG.Models;

public class TradingDealFactory
{
    public string Id { get; set; }
    public string CardToTrade { get; set; }
    public string Type { get; set; }
    public double MinimumDamage { get; set; }
}