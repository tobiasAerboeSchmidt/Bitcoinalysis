using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public interface IAppDbContext
    {
        DbSet<Block> Blocks { get; set; }
        DbSet<TransactionInput> TransactionInputs { get; set; }
        DbSet<TransactionOutput> TransactionOutputs { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        DbSet<Wallet> Wallets { get; set; }

        public Task SaveChangesAsync();
    }
}