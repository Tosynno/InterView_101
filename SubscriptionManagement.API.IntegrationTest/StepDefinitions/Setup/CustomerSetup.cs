using SubscriptionManagement.Application.Models.Request;
using SubscriptionManagement.Application.Repositories;
using SubscriptionManagement.Infrastructure.DataContext;
using TechTalk.SpecFlow;

namespace SubscriptionManagement.API.IntegrationTest.StepDefinitions.Setup
{
    [Binding]
    public class CustomerSetup
    {
        protected LoginRequest request;

        [Given(@"I have a customer data")]
        public void GivenIHaveACustomerData(Table table)
        {
            request = table.CreateInstance<LoginRequest>();
        }

        
        [Given(@"I have a customer in database")]
        public void GivenIHaveACustomerInDatabase(Table table)
        {
            using var context = new SubscriptionManagementDataContext();
            var r = new ServiceRepo(context);

            var c = table.CreateInstance<Domain.Service>();

            GivenCustomerWithEmailDoesNotExists(c.Email);
            r.AddAsync(c).Wait();
        }
    }
}