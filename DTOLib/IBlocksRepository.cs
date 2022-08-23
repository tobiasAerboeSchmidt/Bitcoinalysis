using DTOLib.DTOs;

namespace DTOLib
{
    public interface IBlocksRepository
    {
        Task CreateBlock(List<BlockCreateDTO> dto);
    }
}