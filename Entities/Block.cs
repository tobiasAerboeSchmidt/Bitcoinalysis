using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Block
    {
        [Key]
        public string? Hash { get; set; }

        [Required]
        public DateTime Date { get; set; }


        public ICollection<Transaction>? Transactions { get; set; }

    }
}
