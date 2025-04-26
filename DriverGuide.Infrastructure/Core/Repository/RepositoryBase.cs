namespace DriverGuide.Infrastructure.Core.Repository
{
    public abstract class RepositoryBase
    {
        protected readonly AppDbContext _context;
        protected RepositoryBase(AppDbContext context)
        {
            _context = context;
        }
    }
}
