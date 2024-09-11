using Newtonsoft.Json;

namespace DynamicLib;

public class ExchangeInfo
{
    [JsonProperty("symbols")]
    public List<Symbol>? Symbols { get; set; }
}
