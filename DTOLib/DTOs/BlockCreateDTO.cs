
namespace DTOLib.DTOs
{
    public class BlockCreateDTO
    {
        public string Hash { get; set; }
        public DateTime Date { get; set; }

        public IEnumerable<TransactionDTO> Transactions { get; set; }
    }
}
