using BoDi;
using FunctionalTest.WebApi.Factory;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Diagnostics;
using TechTalk.SpecFlow.Tracing;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace FunctionalTest.WebApi.StepDefinitions
{
    [Binding]
    public class PostControllerStepDefinitions : IClassFixture<AppFactory>
    {
        private AppFactory _factory;
        private HttpClient _client;
        private string? _result;

        public PostControllerStepDefinitions(AppFactory factory) 
        {
            _factory = factory;
            _client = factory.CreateClient();
        }        

        [When(@"Send HTTP GET request to URL ""([^""]*)""")]
        public async Task WhenSendHTTPGETRequestToURL(string url)
        {
            var response = await _client.GetAsync(url);
            _result = await response.Content.ReadAsStringAsync();
        }


        [Then(@"The response should not be empty")]
        public void ThenTheResponseIsNotEmpty()
        {
            _result.Should().NotBeNullOrWhiteSpace();
            
        }
        
    }
}
