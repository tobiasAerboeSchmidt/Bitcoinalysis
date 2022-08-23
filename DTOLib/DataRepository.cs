using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLib
{
    public class DataRepository : RepositoryBase
    {
        public DataRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<int> TestAsync() {
            var numberOfTxs = await _context.Blocks.Include(b => b.Transactions).Skip(1).Take(1).Select(b => b.Transactions.Count).ToListAsync();
            return numberOfTxs.First();
        }

        public async Task<List<Tuple<int, int>>> GetHourBucketsAsync()
        {
            var numberOfWeeks = (int) (_context.Blocks.Max(b => b.Date) - _context.Blocks.Skip(1).Min(b => b.Date)).Days / 7;
            Console.WriteLine(numberOfWeeks);
            var query = await _context.Blocks
                    .Include(b => b.Transactions)
                    .Skip(1)
                    .GroupBy(b => b.Date.Hour)
                    .OrderBy(g => g.Key)
                    .Select(g => new Tuple<int, int>(g.Key, g.Sum(b => b.Transactions.Count()) / (numberOfWeeks * 7)))
                    .ToListAsync();
            return query;
        }

        public async Task<List<Tuple<decimal, int>>> Get4HourBucketsAsync()
        {
            var query = await _context.Blocks
                    .Include(b => b.Transactions)
                    .Skip(1)
                    .GroupBy(b => ((int)b.Date.DayOfWeek * 25) + Math.Floor(b.Date.Hour / 24m))
                    .OrderBy(g => g.Key)
                    .Select(g => new Tuple<decimal, int>(g.Key, g.Sum(b => b.Transactions.Count()) / g.Count()))
                    .ToListAsync();
            
            return query;
        }

        public async Task<List<float>> GetLorenzCurveAsync()
        {
            return null;
        }

        public async Task<List<Tuple<string, int>>> GetWalletOutgoingEdges()
        {
            var transaction_outputs = await _context.TransactionOutputs
                .GroupBy(t => t.OutputWallet)
                .OrderByDescending(g => g.Count())
                .Select(g => new Tuple<string, int>(g.Key, g.Count()))
                .ToListAsync();

            return transaction_outputs;
        }
    }
}
