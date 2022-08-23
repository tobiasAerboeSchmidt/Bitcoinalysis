using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace DataManipulation
{
    public class WalletExplorerClient
    {
        private readonly HttpClient _httpClient;

        public WalletExplorerClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://www.walletexplorer.com");
        }

        public async Task<List<string>> GetAllExchanges()
        {
            var exchangeListQuerySelector = "#main > table > tbody > tr > td:nth-child(1) > ul";
            return await GetAllTypeNames(exchangeListQuerySelector);
        }

        private async Task<List<string>> GetAllTypeNames(string querySelector)
        {
            var response = await _httpClient.GetAsync("");
            var html = await response.Content.ReadAsStringAsync();

            using var context = BrowsingContext.New(Configuration.Default);
            using var doc = await context.OpenAsync(req => req.Content(html));

            var listOfTypeElement = doc.QuerySelector(querySelector);

            var names = listOfTypeElement.ChildNodes.Select(e => e.Text().Split(" ").First());
            return names.ToList();
        }

        public async Task<List<string>> GetAddresses(string name, int lookBackMonths)
        {
            var result = new List<string>();
            var areDatesInRange = await GetLatestDate(name, lookBackMonths);
            if (!areDatesInRange)
            {
                return result;
            }
            var urlPaged = $"/wallet/{name}/addresses?page=";
            var html = await GetHtml(urlPaged + 1);
            var maxPagesElement = html.GetElementsByClassName("paging").First().Children.FirstOrDefault(n => n.Text() == "Last");
            var maxPages = 1;
            if (maxPagesElement is not null)
            {
                maxPages = int.Parse(maxPagesElement.GetAttribute("href").Split("=").Last());
            }
            maxPages = Math.Min(maxPages, 60);
            result.AddRange(ReadAddresses(html));
            for (int i = 2; i <= maxPages; i++)
            {
                Console.WriteLine($"Page {i}/{maxPages}");
                html = await GetHtml(urlPaged + i);
                result.AddRange(ReadAddresses(html));
                Thread.Sleep(500);
            }

            return result;
        }

        private async Task<bool> GetLatestDate(string name, int lookBackMonths)
        {
            var url = $"/wallet/{name}";
            var html = await GetHtml(url);
            var lastDateQuerySelector = "#main > table > tbody > tr:nth-child(2) > td.date";
            var dateElement = html.QuerySelector(lastDateQuerySelector);
            var latestDate = DateTime.Parse(dateElement.Text());
            var diff = DateTime.UtcNow - latestDate;
            return diff.Days < (lookBackMonths * 31);
        }

        private IEnumerable<string> ReadAddresses(IDocument addressPageHtml)
        {
            var addressTableQuerySelector = "#main > table > tbody";
            var tableElement = addressPageHtml.QuerySelector(addressTableQuerySelector);
            var addresses = tableElement.ChildNodes
                .Skip(1)
                .Select(row => row.FirstChild?.FirstChild?.Text()).Where(s => s is not null);
            return addresses;
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
