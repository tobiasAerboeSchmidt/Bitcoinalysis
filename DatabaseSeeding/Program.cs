// See https://aka.ms/new-console-template for more information
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddUserSecrets<Program>()
            .Build();

var connectionString = config.GetConnectionString("DefaultConnection");
var services = new ServiceCollection();
services.AddDbContext<IAppDbContext, AppDbContext>(options =>
options.UseNpgsql(connectionString));

var serviceProvider = services.BuildServiceProvider();
var context = serviceProvider.GetRequiredService<IAppDbContext>();

Console.WriteLine("Clearing all entities...");
context.TransactionInputs.RemoveRange(context.TransactionInputs);
context.TransactionOutputs.RemoveRange(context.TransactionOutputs.Where(t => t.TnxHash != Constants.COINBASE));
context.Transactions.RemoveRange(context.Transactions.Where(t => t.TnxHash != Constants.COINBASE));
context.Blocks.RemoveRange(context.Blocks.Where(b => b.Hash != Constants.COINBASE));
context.Wallets.RemoveRange(context.Wallets.Where(w => w.Address != Constants.COINBASE));

await context.SaveChangesAsync();
Console.WriteLine("done");