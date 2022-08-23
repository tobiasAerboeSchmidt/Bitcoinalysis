using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManipulation
{
    public class BitcoinComClient
    {
        private readonly HttpClient _client;

        public BitcoinComClient()
        {
            _client = new();
            _client.BaseAddress = new Uri("https://blockchain.info/q/");
        }

        public async Task<long> GetTnxFee(string tnx)
        {
            var result = await _client.GetAsync("txfee/" + tnx);
            return long.Parse(await result.Content.ReadAsStringAsync());
        }
    }
}
