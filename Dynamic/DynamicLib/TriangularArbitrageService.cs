using Newtonsoft.Json;

namespace DynamicLib;

public class TriangularArbitrageService
{
    bool _isFirstRun = true;
    private IExchangeClient _exchangeClient;
    private string _filePath = @"C:\Users\dihba\Desktop\Exchanges\DynamicLib\ConsoleApp1\bin\Debug\net8.0\triangular.json";
    public TriangularArbitrageService(IExchangeClient exchangeClient, bool isFirstRun)
    {
        _exchangeClient = exchangeClient;
        _isFirstRun = isFirstRun;
    }
    public async Task WriteTriangularPairs()
    {
        try
        {
            List<List<Pair>> triangularScheme = await DetectTriangularArbitrageAsync();
            var tToJson = JsonConvert.SerializeObject(triangularScheme);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "triangular.json");
            Console.WriteLine($"Attempting to create file at {filePath}");
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.Write(tToJson);
            }
            Console.WriteLine($"File created successfully at {filePath}");
            _filePath = filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating file: {ex.Message}");
        }
    }
    public async Task<List<TriangularTrade>> GetTriangularArbitrageProfitablesAsync()
    {
        List<TriangularTrade> triangularTrades = await ExecuteTriangularTradesAsync();
        var profitables = triangularTrades.Where(tr => tr.IsProfitable);
        return profitables.OrderByDescending(tr => tr.FinalAmount).ToList();
    }
    public async Task<List<TriangularTrade>> ExecuteTriangularTradesAsync()
    {
        List<TriangularCycle> triangularCycles = await TransformToTriangularCylesAsync();
        List<TriangularTrade> triangularTrades = new List<TriangularTrade>();
        Parallel.ForEach(triangularCycles, item =>
        {
            var trade = new TriangularTrade(item, 17.68m);
            lock (triangularTrades)
            {
                triangularTrades.Add(trade);
            }
        });
        return triangularTrades;
    }
    // Done In Both Poloniex And Binance
    private async Task<List<TriangularCycle>> TransformToTriangularCylesAsync()
    {
        List<List<Pair>> triangularScheme = new List<List<Pair>>();
        if (_isFirstRun)
        {
            triangularScheme = await DetectTriangularArbitrageAsync();
        }
        else
        {
            var filePath = _filePath;
            var jsonText = File.ReadAllText(filePath);
            triangularScheme = JsonConvert.DeserializeObject<List<List<Pair>>>(jsonText);
        }
        List<Ticker> tickers = await _exchangeClient.GetTickersAsync();
        List<TriangularCycle> triangularCycles = triangularScheme.Select(item => new TriangularCycle(tickers, item)).ToList();
        return triangularCycles;
    }
    private async Task<List<List<Pair>>> DetectTriangularArbitrageAsync()
    {
        Graph graph = await FullfillGraph();
        TriangularArbitrageDetector detector = new TriangularArbitrageDetector(graph);
        return detector.GetTriangularSchemes();
    }
    private async Task<Graph> FullfillGraph()
    {
        List<Ticker> tickers = await _exchangeClient.GetTickersAsync();
        Graph graph = new Graph();
        foreach (Ticker t in tickers)
        {
            graph.AddCoin(new Coin(t.BaseCoin), new Coin(t.QuoteCoin));
        }
        return graph;
    }
    public async Task<List<Ticker>> GetTickerAsync()
    {
        return await _exchangeClient.GetTickersAsync();
    }
}
