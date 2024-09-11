namespace DynamicLib;

/// <summary>
/// Represents a trading pair consisting of a base coin and a quote coin.
/// </summary>
public class Pair
{
    /// <summary>
    /// Gets or sets the base coin of the trading pair.
    /// </summary>
    public Coin BaseCoin { get; set; }
    /// <summary>
    /// Gets or sets the quote coin of the trading pair.
    /// </summary>
    public Coin QuoteCoin { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="Pair"/> class.
    /// </summary>
    /// <param name="baseCoin">The base coin in the trading pair.</param>
    /// <param name="quoteCoin">The quote coin in the trading pair.</param>
    public Pair(Coin baseCoin, Coin quoteCoin)
    {
        BaseCoin = baseCoin;
        QuoteCoin = quoteCoin;
    }
    /// <summary>
    /// Overrides the default ToString method to facilitate lookup in the TriangularTrade class.
    /// This allows checking if pair prices exist inside the tickers.
    /// </summary>
    /// <returns>A string representation of the pair in the format "BaseCoin_QuoteCoin".</returns>
    public override string ToString()
    {
        return $"{BaseCoin.ToString()}_{QuoteCoin.ToString()}";
    }
    public string ToBinanceString()
    {
        return $"{BaseCoin.ToString()}{QuoteCoin.ToString()}";
    }
}
