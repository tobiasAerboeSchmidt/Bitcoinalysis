using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        static AppDbContext()
        {
        }

        public DbSet<Block> Blocks { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionOutput> TransactionOutputs { get; set; }
        public DbSet<TransactionInput> TransactionInputs { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromSeconds(300));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Block>().HasMany(b => b.Transactions).WithOne(t => t.Block);

            builder.Entity<Transaction>().HasIndex(t => t.BlockHash);
            //builder.Entity<Transaction>().HasMany(t => t.Outputs).WithOne(r => r.Transaction);
            //builder.Entity<Transaction>().HasMany(t => t.Inputs).WithOne(r => r.Transaction);

            //builder.Entity<Wallet>().HasMany(w => w.IncommingTransactions).WithOne(t => t.Wallet);
            //builder.Entity<Wallet>().HasMany(w => w.OutGoingTransactions).WithOne(t => t.Wallet);

            builder.Entity<TransactionInput>().HasKey(ti => new { ti.TnxHash, ti.OutputTnx, ti.OutputIndex });
            builder.Entity<TransactionInput>().HasIndex(ti => ti.InputWallet);
            //builder.Entity<TransactionInput>().HasOne(t => t.Output).WithMany(ot => ot.TransactionInputs).HasForeignKey(t => new { t.OutputTnx, t.OutputIndex });

            builder.Entity<TransactionOutput>().HasKey(to => new { to.TnxHash, to.Index });
            builder.Entity<TransactionOutput>().HasIndex(to => to.OutputWallet);

            builder.Entity<Wallet>().HasData(
                new Wallet { Address = Constants.COINBASE}
                );

            builder.Entity<Block>().HasData(
                new Block { Date = new DateTime(), Hash = Constants.COINBASE }
                );

            builder.Entity<Transaction>().HasData(
                new Transaction { TnxHash = Constants.COINBASE, BlockHash = Constants.COINBASE}
                );

            builder.Entity<TransactionOutput>().HasData(
                new TransactionOutput { Index = -1, TnxHash = Constants.COINBASE, OutputWallet = Constants.COINBASE, Value = 0 }
                );

            base.OnModelCreating(builder);
        }

        public async Task SaveChangesAsync()
        {
            await SaveChangesAsync(default);
        }
    }
}
