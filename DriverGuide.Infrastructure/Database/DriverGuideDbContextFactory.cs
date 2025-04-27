using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DriverGuide.Infrastructure.Database;

public class DriverGuideDbContextFactory : IDesignTimeDbContextFactory<DriverGuideDbContext>
{
    public DriverGuideDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DriverGuideDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new DriverGuideDbContext(optionsBuilder.Options);
    }
}
