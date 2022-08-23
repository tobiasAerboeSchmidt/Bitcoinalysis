using System.Text.Json;

namespace DataManipulation
{
    public static class YChartDailyFeeExtractor
    {
        public static List<DailyTnxFee> Extract(string path)
        {
            var json = "";
            using (StreamReader r = new StreamReader(path))
            {
                json = r.ReadToEnd();
            }

            var obj = JsonSerializer.Deserialize<YChartJsonDTO>(json);

            var result = new List<DailyTnxFee>();
            obj.RawData.ForEach(d =>
                result.Add(new DailyTnxFee
                {
                    Date = (new DateTime(TimeSpan.FromMilliseconds(d[0]).Ticks)).AddYears(1969),
                    Fee = d[1]
                })
            );

            return result;

        }
        public class YChartJsonDTO
        {
            public List<List<double>> RawData { get; set; }

            public YChartJsonDTO(List<List<double>> rawData)
            {
                RawData = rawData;
            }
        }
    }

    public class DailyTnxFee
    {
        public DateTime Date { get; set; }
        public double Fee { get; set; }
    }

}
