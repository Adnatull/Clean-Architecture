using FunctionalTest.WebApi.Factory;

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

        [When(@"GetPosts method is called")]
        public async Task WhenGetPostsMethodIsCalled()
        {
            var response = await _client.GetAsync("/api/v1.0/post");
            _result = await response.Content.ReadAsStringAsync();            
        }

        [Then(@"The response should not be empty")]
        public void ThenTheResponseIsNotEmpty()
        {
            _result.Should().NotBeNullOrWhiteSpace();
        }
        
    }
}
