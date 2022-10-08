using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FunctionalTest.Mvc.Admin
{
    [TestFixture]
    public class PostControllerIntegrationTest
    {
        private AppFactory _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void SetUp()
        {
            _factory = new AppFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task AddPostCommandPostTest()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Admin/Post/Add");
            var formModel = new Dictionary<string, string>
            {
                { "Title", "First Post" },
                { "Slug", "first-post" },
                { "Summary", "This is a first post" },
                { "Content", "This is a big content" }
            };
            postRequest.Content = new FormUrlEncodedContent(formModel);
            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var exists = responseString.Contains("Successfully saved post");
            Assert.AreEqual(true, exists);
        }
    }
}
