namespace DynamicLib;

/// <summary>
/// Represents a graph structure with coins as nodes and their trading relationships as edges.
/// </summary>
public class Graph
{
    private Dictionary<Coin, List<Coin>> _adjacencyList;
    public Graph()
    {
        _adjacencyList = new Dictionary<Coin, List<Coin>>();
    }
    /// <summary>
    /// Adds a base coin and its corresponding quote coin to the graph.
    /// </summary>
    /// <param name="baseCoin">The base coin in the trading pair.</param>
    /// <param name="quoteCoin">The quote coin in the trading pair.</param>
    public void AddCoin(Coin baseCoin, Coin quoteCoin)
    {
        if (!_adjacencyList.ContainsKey(baseCoin))
        {
            _adjacencyList[baseCoin] = new List<Coin>();
        }
        if (!_adjacencyList.ContainsKey(quoteCoin))
        {
            _adjacencyList[quoteCoin] = new List<Coin>();
        }
        if (_adjacencyList[baseCoin].Contains(quoteCoin) || _adjacencyList[quoteCoin].Contains(baseCoin))
        {
            return;
        }
        _adjacencyList[baseCoin].Add(quoteCoin);
        _adjacencyList[quoteCoin].Add(baseCoin);
    }
    /// <summary>
    /// Gets the list of quote coins that can be traded with the specified base coin.
    /// </summary>
    /// <param name="coin">The base coin.</param>
    /// <returns>A list of quote coins.</returns>
    public List<Coin> GetQuotes(Coin coin)
    {
        return _adjacencyList.ContainsKey(coin) ? _adjacencyList[coin] : new List<Coin>();
    }
    /// <summary>
    /// Gets the list of all base coins (nodes) in the graph.
    /// </summary>
    /// <returns>A list of all base coins.</returns>
    public List<Coin> GetKeys()
    {
        return _adjacencyList.Keys.ToList();
    }
}
