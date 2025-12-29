using Xunit;

namespace DriverGuide.Tests;

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}

public class DatabaseFixture : IDisposable
{
    public DatabaseFixture()
    {
    }

    public void Dispose()
    {
    }
}
