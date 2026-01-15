using Bogus;
using fields.services;

namespace fields.@base
{
    public abstract class BaseTest
    {
        protected HttpClient Client => ApiClientSingleton.Instance;
        protected SimpleLogger Logger => SimpleLogger.Log;

        [SetUp]
        public virtual async Task BaseSetUp()
        {
            Logger.Info("Test setup: starting test");
            await Task.CompletedTask; 
        }

        [TearDown]
        public virtual void BaseTearDown()
        {
            Logger.Info("Test teardown: finished test");
        }
    }
    
    public abstract class BaseApiTest : BaseTest
    {
        protected PasswordRecoveryService PasswordRecoveryService { get; private set; }
        protected Faker Faker { get; private set; }

        [SetUp]
        public void ApiTestSetup()
        {
            PasswordRecoveryService = new PasswordRecoveryService(Client, AppConfig.BaseUrl);
            Faker = new Faker("en");
        }
    }
}