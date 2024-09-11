
namespace DynamicLib;

/// <summary>
/// Represents a triangular arbitrage trade, including the cycle and trading records.
/// </summary>
public class TriangularTrade
{
    private decimal _finalAmount;
    private decimal _amount;
    private decimal _nextAmount;
    private TriangularCycle _tradeCycle;
    private decimal _triangularProfitAmount;
    /// <summary>
    /// Gets the triangular cycle associated with this trade.
    /// </summary>
    public TriangularCycle TriangularCycle => _tradeCycle;
    /// <summary>
    /// Gets or sets the list of trading records for this trade.
    /// </summary>
    public List<TradingRecord> TradingRecords { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this trade is profitable.
    /// </summary>
    public bool IsProfitable { get; set; }
    /// <summary>
    /// Gets the value of the final Amount of First Coin
    /// </summary>
    public decimal FinalAmount => _finalAmount;
    public decimal TriangularProfitAmount => _triangularProfitAmount;
    /// <summary>
    /// Initializes a new instance of the <see cref="TriangularTrade"/> class with the specified triangular cycle.
    /// </summary>
    /// <param name="tradingCycle">The triangular cycle to use for this trade.</param>
    public TriangularTrade(TriangularCycle tradingCycle, decimal amount)
    {
        _amount = amount;
        _tradeCycle = tradingCycle;
        TradingRecords = new List<TradingRecord>() { TradingPair(TriangularCycle.FirstPair, _amount), TradingPair(TriangularCycle.SecondPair, _nextAmount), TradingPair(TriangularCycle.ThirdPair, _nextAmount) };
        IsProfitable = CheckProfit();
        _finalAmount = TradingRecords.Last().EndingAmount;
        _triangularProfitAmount = _finalAmount - _amount;
    }
    /// <summary>
    /// Determines whether the trade is profitable by comparing the ending amount of the last trade
    /// with the starting amount of the first trade.
    /// </summary>
    /// <returns><c>true</c> if the trade is profitable; otherwise, <c>false</c>.</returns>
    private bool CheckProfit()
    {
        return TradingRecords.Last().EndingAmount > TradingRecords.First().StartingAmount;
    }

    /// <summary>
    /// Executes a trade for a given pair, calculating the final result after exchange fees.
    /// </summary>
    /// <param name="pair">The currency pair to trade.</param>
    /// <param name="amount">The amount to trade. Default is 1.</param>
    /// <param name="exchangeFees">The exchange fee percentage. Default is 15.5m = 0.155%.</param>
    /// <returns>A trading record with the results of the trade.</returns>
    private TradingRecord TradePair(Pair pair, decimal amount = 1, decimal exchangeFees = 0.2m)
    {
        int pairIndex = TriangularCycle.TradingSymbols.IndexOf(pair.ToString());
        string pairSymbol = pairIndex != -1 ? TriangularCycle.TradingSymbols[pairIndex] : TriangularCycle.GetTradingSymbol(pair);
        TradingRates rates = pairIndex != -1 ? TriangularCycle.TradingRates[pairIndex] : TriangularCycle.GetRate(pairSymbol);
        TradingDirection direction = pairIndex != -1 ? TriangularCycle.Directions[pairIndex] : TriangularCycle.SetTradingDirection(pair);

        decimal takerFee = exchangeFees / 100;
        decimal resultWithoutFee, finalResult;

        if (direction == TradingDirection.LeftToRigth)
        {
            resultWithoutFee = amount * rates.AskPrice;
            finalResult = resultWithoutFee * (1 - takerFee);
        }
        else
        {
            resultWithoutFee = amount * (1 / rates.BidPrice);
            finalResult = resultWithoutFee * (1 - takerFee);
        }
        _nextAmount = finalResult;
        return new TradingRecord
        {
            OriginalPair = pair,
            Symbol = pairSymbol,
            BidPrice = rates.BidPrice,
            AskPrice = rates.AskPrice,
            Direction = direction,
            StartingCoin = pair.BaseCoin,
            StartingAmount = amount,
            EndingCoin = pair.QuoteCoin,
            EndingAmount = finalResult,
            FeesAmount = resultWithoutFee - finalResult
        };
    }
    private TradingRecord TradingPair(Pair pair, decimal amount = 1, decimal exchangeFee = 0.2m)
    {
        bool isBinanceCylce = TriangularCycle.IsBinanceCycle;
        if (isBinanceCylce)
        {
            return HandleBinanceTrade(pair, amount, 0.1m);
        }
        return HandlePoloniexTrade(pair, amount, 0.2m);
    }

    private TradingRecord HandleBinanceTrade(Pair pair, decimal amount, decimal exchangeFee)
    {
        int pairIndex = TriangularCycle.TradingSymbols.IndexOf(pair.ToBinanceString());
        string pairSymbol = pairIndex != -1 ? TriangularCycle.TradingSymbols[pairIndex] : TriangularCycle.GetTradingSymbol(pair);
        TradingRates rates = pairIndex != -1 ? TriangularCycle.TradingRates[pairIndex] : TriangularCycle.GetRate(pairSymbol);
        TradingDirection direction = pairIndex != -1 ? TriangularCycle.Directions[pairIndex] : TriangularCycle.SetTradingDirection(pair);

        decimal takerFee = exchangeFee / 100;
        decimal resultWithoutFee, finalResult, amountAfterFees;
        decimal fees = amount * takerFee;
        amountAfterFees = amount - fees;
        if (direction == TradingDirection.RightToLeft)
        {
            // resultWithoutFee = amount / rates.AskPrice;
            finalResult = amountAfterFees / rates.AskPrice;
        }
        else
        {
            // resultWithoutFee = amount * rates.BidPrice;
            finalResult = amountAfterFees * rates.BidPrice;
        }
        //finalResult = resultWithoutFee * (1 - takerFee);
        
        _nextAmount = finalResult;

        return new TradingRecord
        {
            OriginalPair = pair,
            Symbol = pairSymbol,
            BidPrice = rates.BidPrice,
            AskPrice = rates.AskPrice,
            Direction = direction,
            StartingCoin = pair.BaseCoin,
            StartingAmount = amount,
            EndingCoin = pair.QuoteCoin,
            EndingAmount = finalResult,
            //FeesAmount = resultWithoutFee - finalResult
            FeesAmount = fees
        };
    }
    private TradingRecord HandlePoloniexTrade(Pair pair, decimal amount, decimal exchangeFee )
    {
        int pairIndex = TriangularCycle.TradingSymbols.IndexOf(pair.ToString());
        string pairSymbol = pairIndex != -1 ? TriangularCycle.TradingSymbols[pairIndex] : TriangularCycle.GetTradingSymbol(pair);
        TradingRates rates = pairIndex != -1 ? TriangularCycle.TradingRates[pairIndex] : TriangularCycle.GetRate(pairSymbol);
        TradingDirection direction = pairIndex != -1 ? TriangularCycle.Directions[pairIndex] : TriangularCycle.SetTradingDirection(pair);

        decimal takerFee = exchangeFee / 100;
        decimal resultWithoutFee, finalResult;

        if (direction == TradingDirection.LeftToRigth)
        {
            resultWithoutFee = amount * rates.AskPrice;
        }
        else
        {
            resultWithoutFee = amount * (1 / rates.BidPrice);
        }
        finalResult = resultWithoutFee * (1 - takerFee);

        _nextAmount = finalResult;

        return new TradingRecord
        {
            OriginalPair = pair,
            Symbol = pairSymbol,
            BidPrice = rates.BidPrice,
            AskPrice = rates.AskPrice,
            Direction = direction,
            StartingCoin = pair.BaseCoin,
            StartingAmount = amount,
            EndingCoin = pair.QuoteCoin,
            EndingAmount = finalResult,
            FeesAmount = resultWithoutFee - finalResult
        };
    }

}
