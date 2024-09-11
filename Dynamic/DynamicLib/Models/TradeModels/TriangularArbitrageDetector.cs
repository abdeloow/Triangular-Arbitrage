namespace DynamicLib;

/// <summary>
/// Detects triangular arbitrage opportunities within a graph of trading pairs.
/// </summary>
public class TriangularArbitrageDetector
{
    private readonly Graph _graph;
    /// <summary>
    /// Initializes a new instance of the <see cref="TriangularArbitrageDetector"/> class.
    /// </summary>
    /// <param name="graph">The graph representing the trading pairs.</param>
    public TriangularArbitrageDetector(Graph graph) => _graph = graph;
    /// <summary>
    /// Gets a list of all triangular schemes (arbitrage opportunities) within the graph.
    /// </summary>
    public List<List<Pair>> GetTriangularSchemes()
    {
        List<List<Pair>> triangularList = new List<List<Pair>>();
        foreach (Coin firstCoin in _graph.GetKeys())
        {
            foreach (Coin quoteCoin in _graph.GetQuotes(firstCoin))
            {
                Pair pair1 = new Pair(firstCoin, quoteCoin);
                foreach (Coin thirdCoin in _graph.GetQuotes(quoteCoin))
                {
                    Pair pair2 = new Pair(quoteCoin, thirdCoin);
                    if (_graph.GetQuotes(thirdCoin).Contains(firstCoin))
                    {
                        Pair pair3 = new Pair(thirdCoin, firstCoin);
                        triangularList.Add(new List<Pair>() { pair1, pair2, pair3 });
                    }
                }
            }
        }
        return triangularList;
    }
}
