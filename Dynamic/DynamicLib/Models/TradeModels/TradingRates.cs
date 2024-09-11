namespace DynamicLib;

/// <summary>
/// Represents a trading record with high bid price, low ask price, and the result of the trade.
/// </summary>
public record TradingRates
{
    public decimal BidPrice;
    public decimal AskPrice;
}
