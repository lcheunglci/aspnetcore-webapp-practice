namespace GigHub.IntegrationTests
{
    public class TestBase : IDisposable
    {
        // https://stackoverflow.com/questions/12976319/xunit-net-global-setup-teardown

        protected TestBase()
        {
            // Global Initialization happens here.  Called before every test.
            // var configuration = new GigHub.Migrations


        }

        public void Dispose()
        {
            // Global Clean up happens here.  Called after every test.
        }
    }
}