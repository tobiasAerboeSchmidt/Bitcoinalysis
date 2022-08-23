using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Wallet
    {
        [Key]
        public string? Address { get; set; }

        //public ICollection<TransactionOutput>? OutGoingTransactions { get; set; }
        //public ICollection<TransactionInput>? IncommingTransactions { get; set; }
    }
}
