using Newtonsoft.Json;
namespace DynamicLib;

public class PoloniexTicker
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = default!;
    [JsonProperty("bid")]
    public string? BestBidPrice { get; set; }
    [JsonProperty("ask")]
    public string? BestAskPrice { get; set; }
    [JsonProperty("tradeCount")]
    public int TradeCount { get; set; }
}
