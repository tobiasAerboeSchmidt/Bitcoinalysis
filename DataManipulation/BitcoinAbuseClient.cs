using AngleSharp.Dom;
using AngleSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataManipulation
{
    public class BitcoinAbuseClient
    {
        private readonly HttpClient _httpClient;

        public BitcoinAbuseClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://www.bitcoinabuse.com/");
        }

        public async Task<List<string>> GetAddresses(int pages = 218)
        {
            List<string> addresses = new List<string>();
            var url = "reports?page=";
            for (int i = 1; i <= pages; i++)
            {
                Console.WriteLine($"page {i}/{pages}");
                var html = await GetHtml(url + i);
                addresses.AddRange(ExtractAddresses(html));
            }
            return addresses;
        }

        private IEnumerable<string> ExtractAddresses(IDocument tablePage)
        {
            var tableQuerySelector = "body > div:nth-child(1) > main > div.container > div:nth-child(6)";
            var tableElement = tablePage.QuerySelector(tableQuerySelector);
            return tableElement.Children.Where(n => n.HasChildNodes).Select(n => n.Children.First().Text());
        }

        private async Task<IDocument> GetHtml(string url)
        {
            var backoffCount = 0;
            while (true)
            {
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    backoffCount++;
                    var seconds = 60000;
                    Console.WriteLine($"Too many request, waiting {seconds / 1000} seconds...");
                    Thread.Sleep(seconds);
                    continue;
                }
                var html = await response.Content.ReadAsStringAsync();

                var context = BrowsingContext.New(Configuration.Default);
                return await context.OpenAsync(req => req.Content(html));
            }
        }
    }
}
