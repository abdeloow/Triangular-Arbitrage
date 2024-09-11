namespace DynamicLib;

public class Ticker
{
    public string Symbol { get; set; } = default!;
    public string? BaseCoin { get; set; }
    public string? QuoteCoin { get; set; }
    public decimal BidPrice { get; set; }
    public decimal AskPrice { get; set; }
    public bool IsPoloniexTicker { get; set; }
    public bool IsBinanceTicker { get; set; }
}
