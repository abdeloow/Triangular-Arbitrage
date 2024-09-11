using DynamicLib;



Console.WriteLine("Starting Poloniex");
TriangularArbitrageService pService = new TriangularArbitrageService(new PoloniexClient(), true);
var result2 = await pService.GetTriangularArbitrageProfitablesAsync();
result2 = result2.OrderByDescending(tr => tr.FinalAmount).ToList();
Console.WriteLine("Done");

Console.ReadLine();