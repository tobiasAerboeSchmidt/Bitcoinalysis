// See https://aka.ms/new-console-template for more information
using DataManipulation;

//var data = YChartDailyFeeExtractor.Extract("./ressources/ycharts-btc-tnx-fee-per-day-1-year.json");

//var grouped = data.GroupBy(d => d.Date.DayOfWeek);
//var sum = grouped.Select(g => new Tuple<DayOfWeek,float>(g.Key, (float) g.Average(g => g.Fee))).ToList();
//data.ForEach(d =>
//{
//    Console.WriteLine(d.Date.ToString("yy-M-d"));
//});
//data.ForEach(d =>
//{
//    Console.WriteLine(d.Fee);
//});

//var data = LorenzExtraction.LoadData("./ressources/walletCount.txt");
//data = data.Where(d => d.Item2 > 1).ToList();
//Console.WriteLine(data.Count);
//Console.WriteLine(data.Sum(d => d.Item2));

//var lorenzCurveData = LorenzExtraction.CalculateLorenzCurveData(data);
//for (int i = 0; i < lorenzCurveData.Length; i++)
//{
//    Console.WriteLine(lorenzCurveData[i]);
//}
var client = new BitcoinAbuseClient();
var addresses = await client.GetAddresses();
foreach (var address in addresses)
{
    Console.WriteLine(address);

}
File.WriteAllLines($"BitcoinAbuseAddresses.txt", addresses.Select(x => string.Join(",\n", x)));
