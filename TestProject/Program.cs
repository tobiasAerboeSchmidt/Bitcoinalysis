// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.Linq;
using DTOLib;
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
services.AddScoped<DataRepository>();
var serviceProvider = services.BuildServiceProvider();

var repository = serviceProvider.GetRequiredService<DataRepository>();

//var buckets = await repository.GetHourBucketsAsync();

//foreach (var bucket in buckets)
//{
//Console.WriteLine(bucket.Item1 + ", " + bucket.Item2);

//}
Console.WriteLine(DateTime.Now);

var outgoingWalletEdges = await repository.GetWalletOutgoingEdges();


Console.WriteLine(outgoingWalletEdges.Count());
Console.WriteLine(outgoingWalletEdges.First());

var walletCountList = outgoingWalletEdges.Select(x => x.Item1 + ", " + x.Item2).ToArray();

File.WriteAllLines("walletCount.txt", walletCountList.Select(x => string.Join(",", x)));