using Newtonsoft.Json;

namespace DynamicLib;

public class PoloniexClient : IExchangeClient
{
    const string ticker24hApiUrl = "https://api.poloniex.com/markets/ticker24h";
    public async Task<List<Ticker>> GetTickersAsync()
    {
        List<Ticker> tickers = new List<Ticker>();
        string content = await GetPoloniexDataAsync();
        List<PoloniexTicker> poloniexTickers = JsonConvert.DeserializeObject<List<PoloniexTicker>>(content);
        if (poloniexTickers != null)
        {
            tickers = poloniexTickers.Where(pt => pt.TradeCount > 20).Select(pt => new Ticker()
            {
                Symbol = pt.Symbol,
                BaseCoin = pt.Symbol.Split('_')[0],
                QuoteCoin = pt.Symbol.Split('_')[1],
                AskPrice = Convert.ToDecimal(pt.BestAskPrice),
                BidPrice = Convert.ToDecimal(pt.BestBidPrice),
                IsPoloniexTicker = true
            }).ToList();
        }
        return tickers;
    }
    private async Task<string> GetPoloniexDataAsync()
    {
        HttpClient client = new HttpClient();
        string content = string.Empty;
        try
        {
            HttpResponseMessage message = await client.GetAsync(ticker24hApiUrl);
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
