using Newtonsoft.Json;

namespace DynamicLib;
/// <summary>
/// The BinanceClient class is responsible for interacting with the Binance API.
/// It retrieves market data such as ticker information and symbols from the exchange.
/// </summary>
public class BinanceClient : IExchangeClient
{
    const string _exchangeInfo = "https://api.binance.com/api/v3/exchangeInfo";
    const string _bianceTicker24h = "https://api.binance.com/api/v3/ticker/24hr";
    /// <summary>
    /// Retrieves a list of tickers from the Binance API.
    /// Each ticker includes the symbol, base coin, quote coin, bid price, ask price, and a flag indicating it's from Binance.
    /// </summary>
    /// <returns>A list of Ticker objects with data from Binance.</returns>
    public async Task<List<Ticker>> GetTickersAsync()
    {
        List<Ticker> tickers = new List<Ticker>();
        List<Symbol> symbols = await GetSymbolsAsync();
        List<BinanceTicker> binanceTickers = await GetBTickersAsync();
        foreach (var symbol in symbols)
        {
            foreach (var bticker in binanceTickers)
            {
                if (symbol.FullName == bticker.Symbol)
                {
                    tickers.Add(new Ticker
                    {
                        Symbol = symbol.FullName,
                        BaseCoin = symbol.BaseAsset,
                        QuoteCoin = symbol.QuoteAsset,
                        BidPrice = bticker.BidPrice,
                        AskPrice = bticker.AskPrice,
                        IsBinanceTicker = true
                    });
                }
            }
        }
        return tickers;
    }
    /// <summary>
    /// Retrieves a list of Binance tickers, filtered by a minimum number of trades.
    /// </summary>
    /// <returns>A list of BinanceTicker objects with data from Binance.</returns>
    private async Task<List<BinanceTicker>> GetBTickersAsync()
    {
        List<BinanceTicker> tickers = new List<BinanceTicker>();
        string content = await GetInfoAsync(_bianceTicker24h);
        tickers = JsonConvert.DeserializeObject<List<BinanceTicker>>(content);
        return tickers ?? tickers.Where(bt => bt.TradesCount > 500).ToList();
    }
    /// <summary>
    /// Retrieves a list of symbols that are currently trading on Binance.
    /// </summary>
    /// <returns>A list of Symbol objects representing tradable pairs on Binance.</returns>
    private async Task<List<Symbol>> GetSymbolsAsync()
    {
        List<Symbol> symbols = new List<Symbol>();
        string content = await GetInfoAsync(_exchangeInfo);
        ExchangeInfo exchangeInfo = JsonConvert.DeserializeObject<ExchangeInfo>(content);
        if (exchangeInfo != null)
        {
            symbols = exchangeInfo.Symbols.Where(s => s.Status == "TRADING").ToList();
        }
        return symbols;
    }
    /// <summary>
    /// Sends an HTTP GET request to the provided URL and returns the response content as a string.
    /// </summary>
    /// <param name="url">The URL to send the request to.</param>
    /// <returns>The content of the response as a string.</returns>
    private async Task<string> GetInfoAsync(string url)
    {
        HttpClient client = new HttpClient();
        string content = string.Empty;
        try
        {
            HttpResponseMessage message = await client.GetAsync(url);
            message.EnsureSuccessStatusCode();
            if (message.IsSuccessStatusCode)
            {
                content = await message.Content.ReadAsStringAsync();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
        client.Dispose();
        return content;
    }

}
