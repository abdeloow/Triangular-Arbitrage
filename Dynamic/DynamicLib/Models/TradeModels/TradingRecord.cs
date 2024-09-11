namespace DynamicLib;

/// <summary>
/// Represents a record of a trade within a triangular arbitrage sequence.
/// </summary>
public record TradingRecord
{
    /// <summary>
    /// Gets the original trading pair involved in the trade.
    /// </summary>
    public Pair OriginalPair { get; init; }
    /// <summary>
    /// Gets the symbol of the trading pair.
    /// </summary>
    public string Symbol { get; init; }
    /// <summary>
    /// Gets the highest bid price for the trade.
    /// </summary>
    public decimal BidPrice { get; init; }
    /// <summary>
    /// Gets the lowest ask price for the trade.
    /// </summary>
    public decimal AskPrice { get; init; }
    /// <summary>
    /// Gets the direction of the trade (buying or selling).
    /// </summary>
    public TradingDirection Direction { get; init; }
    /// <summary>
    /// Gets the starting amount of the base coin before the trade.
    /// </summary>
    public decimal StartingAmount { get; init; }
    /// <summary>
    /// Gets the coin that was used to start the trade.
    /// </summary>
    public Coin StartingCoin { get; init; }
    /// <summary>
    /// Gets the ending amount of the quote coin after the trade.
    /// </summary>
    public decimal EndingAmount { get; init; }
    /// <summary>
    /// Gets the coin that was received after completing the trade.
    /// </summary>
    public Coin EndingCoin { get; init; }
    /// <summary>
    /// Gets the amount of fees deducted from the trade.
    /// </summary>
    public decimal FeesAmount { get; init; }
}
