using Xunit;

namespace ASP.NET_Core_Webapp.IntegrationTests.Fixtures
{
    [CollectionDefinition("BaseCollection")]
    public class Collection : ICollectionFixture<TestContext>
    {
    }
}
