namespace DynamicLib;


/// <summary>
/// Represents a triangular arbitrage cycle, which includes three currency pairs and handles 
/// the determination of trading symbols, directions, and rates for each pair.
/// </summary>
public class TriangularCycle
{
    private List<Ticker> _tickers;
    private readonly Pair _firstPair;
    private readonly Pair _secondPair;
    private readonly Pair _thirdPair;
    private bool _isBinanceCycle = false;
    /// <summary>
    /// Gets the first pair in the triangular arbitrage cycle.
    /// </summary>
    public Pair FirstPair => _firstPair;
    /// <summary>
    /// Gets the second pair in the triangular arbitrage cycle.
    /// </summary>
    public Pair SecondPair => _secondPair;
    /// <summary>
    /// Gets the third pair in the triangular arbitrage cycle.
    /// </summary>
    public Pair ThirdPair => _thirdPair;
    /// <summary>
    /// Gets or sets the list of pairs involved in the triangular arbitrage cycle.
    /// </summary>
    public List<Pair> TriangularTrades { get; set; }
    /// <summary>
    /// Gets the list of trading directions for each pair in the triangular cycle.
    /// </summary>
    public List<TradingDirection> Directions { get; private set; }
    /// <summary>
    /// Gets the list of trading symbols for each pair in the triangular cycle.
    /// </summary>
    public List<string> TradingSymbols { get; init; }
    /// <summary>
    /// Gets or sets the trading rates for each pair in the triangular cycle.
    /// </summary>
    public List<TradingRates> TradingRates { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="TriangularCycle"/> class with the specified tickers and pairs.
    /// </summary>
    /// <param name="tickers">The list of tickers containing market data.</param>
    /// <param name="triangularTrades">The list of pairs that form the triangular arbitrage cycle.</param>
    public bool IsBinanceCycle => _isBinanceCycle;
    public TriangularCycle(List<Ticker> tickers, List<Pair> triangularTrades)
    {
        _tickers = tickers;
        _isBinanceCycle = _tickers.Any(t => t.IsBinanceTicker);
        TriangularTrades = triangularTrades;
        _firstPair = triangularTrades.First();
        _secondPair = triangularTrades[1];
        _thirdPair = triangularTrades.Last();
        Directions = new List<TradingDirection>() { SetTradingDirection(FirstPair), SetTradingDirection(SecondPair), SetTradingDirection(ThirdPair) };
        TradingSymbols = new List<string>() { GetTradingSymbol(FirstPair), GetTradingSymbol(SecondPair), GetTradingSymbol(ThirdPair) };
        TradingRates = GetRates();
    }
    /// <summary>
    /// Sets the trading direction for a given pair based on whether it exists in the tickers list.
    /// </summary>
    /// <param name="pair">The currency pair to determine the direction for.</param>
    /// <returns>The trading direction for the pair.</returns>
    public TradingDirection SetTradingDirection(Pair pair)
    {
        return IsPairExists(pair) ? TradingDirection.LeftToRigth : TradingDirection.RightToLeft;
    }
    /// <summary>
    /// Gets the trading symbol for a given pair, reversing it if necessary.
    /// </summary>
    /// <param name="pair">The currency pair to get the trading symbol for.</param>
    /// <returns>The trading symbol for the pair.</returns>
    public string GetTradingSymbol(Pair pair)
    {
        if (_isBinanceCycle)
        {
            if (IsPairExists(pair))
            {
                return pair.ToBinanceString();
            }
            return ReversePair(pair);
        }
        return IsPairExists(pair) ? pair.ToString() : ReversePair(pair);
    }
    /// <summary>
    /// Determines whether a given pair exists in the tickers list.
    /// </summary>
    /// <param name="pair">The currency pair to check.</param>
    /// <returns><c>true</c> if the pair exists; otherwise, <c>false</c>.</returns>
    public bool IsPairExists(Pair pair)
    {
        /*if (_isBinancePair)
        {
            return _tickers.Any(t => t.Symbol == pair.ToBinanceString());
        }
        return _tickers.Any(t => t.Symbol == pair.ToString());*/
        return _isBinanceCycle ? _tickers.Any(t => t.Symbol == pair.ToBinanceString()) : _tickers.Any(t => t.Symbol == pair.ToString());
    }
    /// <summary>
    /// Reverses a given currency pair.
    /// </summary>
    /// <param name="pair">The currency pair to reverse.</param>
    /// <returns>The reversed pair as a string.</returns>
    public string ReversePair(Pair pair)
    {
        return _isBinanceCycle ? new Pair(pair.QuoteCoin, pair.BaseCoin).ToBinanceString() : new Pair(pair.QuoteCoin, pair.BaseCoin).ToString();
    }
    /// <summary>
    /// Gets the trading rates (bid and ask prices) for a given symbol.
    /// </summary>
    /// <param name="symbol">The trading symbol to get rates for.</param>
    /// <returns>The trading rates for the symbol.</returns>
    public TradingRates GetRate(string symbol)
    {
        var ticker = _tickers.FirstOrDefault(t => t.Symbol == symbol);
        TradingRates rates = new TradingRates();
        if (ticker != null)
        {
            rates.BidPrice = ticker.BidPrice;
            rates.AskPrice = ticker.AskPrice;
        }
        return rates;
    }
    /// <summary>
    /// Gets the trading rates (bid and ask prices) for all pairs in the triangular cycle.
    /// </summary>
    /// <returns>A list of trading rates for each pair in the cycle.</returns>
    public List<TradingRates> GetRates()
    {
        return TradingSymbols.Select(symbol => GetRate(symbol)).ToList();
    }
}
