using System.Transactions;

namespace GigHub.IntegrationTests
{
    public class TestBase : IDisposable
    {
        // https://stackoverflow.com/questions/12976319/xunit-net-global-setup-teardown
        // https://learn.microsoft.com/en-us/sql/ssdt/how-to-write-sql-server-unit-test-that-runs-in-single-transaction-scope?view=sql-server-ver16
        TransactionScope _trans;

        protected TestBase()
        {
            // Global Initialization happens here.  Called before every test.
            // var configuration = new GigHub.Migrations
            _trans = new TransactionScope();
        }

        public virtual void Dispose()
        {
            // Global Clean up happens here.  Called after every test.
            _trans.Dispose();
        }
    }
}