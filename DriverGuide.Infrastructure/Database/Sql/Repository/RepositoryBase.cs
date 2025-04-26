namespace DriverGuide.Infrastructure.Database;

public abstract class RepositoryBase
{
    protected AppDbContext Context { get; private set; }
    protected RepositoryBase(AppDbContext context)
    {
        Context = context;
    }
}
