using Newtonsoft.Json;

namespace DynamicLib;

public class BinanceTicker
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = default!;
    [JsonProperty("bidPrice")]
    public decimal BidPrice { get; set; }
    [JsonProperty("askPrice")]
    public decimal AskPrice { get; set; }
    [JsonProperty("count")]
    public int TradesCount { get; set; }
}
