using Newtonsoft.Json;

namespace DynamicLib;

public class Symbol
{
    [JsonProperty("symbol")]
    public string? FullName { get; set; }
    [JsonProperty("status")]
    public string? Status { get; set; }
    [JsonProperty("baseAsset")]
    public string? BaseAsset { get; set; }
    [JsonProperty("quoteAsset")]
    public string? QuoteAsset { get; set; }
}
