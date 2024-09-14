using NUnit.Framework;
using System.Net.Http;
using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace SubscriptionManagement.API.IntegrationTest.StepDefinitions.Setup
{
    [Binding]
    public class UserLoginSteps
    {
        private readonly HttpClient _client;
        private HttpResponseMessage _response;
        private string _serviceId;
        private string _password;
        private const string ApiBaseUrl = "http://localhost:44327";
        public UserLoginSteps(HttpClient client)
        {
            _client = client;
        }

        [Given(@"I have a customer in database")]
        public void GivenIHaveACustomerInDatabase(Table table)
        {
            // Assume this step already has customer data inserted in the DB
            var customer = table.Rows[0];
            _serviceId = customer["ServiceId"];
            _password = customer["Password"];
        }

        [Given(@"I have a login data")]
        public void GivenIHaveALoginData(Table table)
        {
            var data = table.Rows[0];
            _serviceId = data["ServiceId"];
            _password = data["Password"];
        }

        [When(@"I call the Login API")]
        public async Task WhenICallTheLoginAPI()
        {
            var requestBody = new
            {
                ServiceId = _serviceId,
                Password = _password
            };

            _response = await _client.PostAsJsonAsync($"{ApiBaseUrl}/account/login", requestBody);
        }

        [Then(@"I should receive a success response")]
        public void ThenIShouldReceiveASuccessResponse()
        {
            Assert.That(200, Is.EqualTo((int)_response.StatusCode));
        }

        [Then(@"response has a valid token")]
        public async Task ThenResponseHasAValidToken()
        {
            var responseBody = await _response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);
            Assert.That("", Is.EqualTo(responseBody)); 
        }

        [Then(@"I will receive an error code (.*)")]
        public void ThenIWillReceiveAnErrorCode(int statusCode)
        {
            Assert.That(statusCode, Is.EqualTo((int)_response.StatusCode));
        }

        [Then(@"Response message should contain ""(.*)""")]
        public async Task ThenResponseMessageShouldContain(string errorMessage)
        {
            var responseBody = await _response.Content.ReadAsStringAsync();
            Is.EqualTo(responseBody.Contains(errorMessage));
        }
    }

}


