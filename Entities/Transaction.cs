using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Transaction
    {
        [Key]
        [Required]
        public string? TnxHash { get; set; }

        [Required]
        [ForeignKey(nameof(Entities.Block))]
        public string? BlockHash { get; set; }


        //public Block? Block { get; set; }
        //public ICollection<TransactionOutput> Outputs { get; set; }
        //public ICollection<TransactionInput> Inputs { get; set; }
    }
}