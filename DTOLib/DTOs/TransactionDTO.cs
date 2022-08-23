using Entities;

namespace DTOLib.DTOs
{
    public class TransactionDTO
    {
        public string Hash { get; set; }
        public IEnumerable<TransactionInputDTO> Inputs { get; set; }
        public IEnumerable<TransactionOutputDTO> Outputs { get; set; }
    }

    public class TransactionInputDTO
    {
        public string OutputTnx { get; set; }
        public int OutputIndex { get; set; }
    }

    public class TransactionOutputDTO
    {
        public string Address { get; set; }
        public long Value { get; set; }
        public int Index { get; set; }

        public TransactionOutputDTO()
        {
            Address = Constants.COINBASE;
        }
    }
}