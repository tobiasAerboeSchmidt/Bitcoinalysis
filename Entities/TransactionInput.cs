using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class TransactionInput
    {
        [Required]
        //[ForeignKey(nameof(Entities.Transaction))]
        public string? TnxHash { get; set; }

        [Required]
        public int OutputIndex { get; set; }

        [Required]
        public string? OutputTnx { get; set; }

        //[ForeignKey(nameof(Entities.Wallet))]
        public string? InputWallet { get; set; }

        //public Transaction? Transaction { get; set; }

        //public Wallet? Wallet { get; set; }

        //public TransactionOutput? Output { get; set; }
    }
}
