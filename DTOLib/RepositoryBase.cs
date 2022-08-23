using Entities;

namespace DTOLib
{
    public class RepositoryBase
    {
        protected readonly AppDbContext _context;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }
    }
}
