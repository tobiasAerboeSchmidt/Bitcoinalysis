using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class TransactionOutput
    {
        [Required]
        //[ForeignKey(nameof(Entities.Transaction))]
        public string? TnxHash { get; set; }

        [Required]
        public int Index { get; set; }

        [Required]
        public long Value { get; set; }

        [Required]
        //[ForeignKey(nameof(Entities.Wallet))]
        public string? OutputWallet { get; set; }



        //public Wallet? Wallet { get; set; }
        //public Transaction? Transaction { get; set; }
        //public ICollection<TransactionInput>? TransactionInputs { get; set; }
    }
}
