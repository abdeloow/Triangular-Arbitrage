namespace DynamicLib;

public interface IExchangeClient
{
    Task<List<Ticker>> GetTickersAsync();
}
