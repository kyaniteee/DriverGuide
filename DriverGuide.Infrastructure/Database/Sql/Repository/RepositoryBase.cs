namespace DriverGuide.Infrastructure.Database;

public abstract class RepositoryBase
{
    protected DriverGuideDbContext Context { get; private set; }
    protected RepositoryBase(DriverGuideDbContext context)
    {
        Context = context;
    }
}
