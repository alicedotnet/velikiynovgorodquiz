using Microsoft.AspNetCore.Mvc.Testing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using VNQuiz.Alice.Tests.TestsInfrastructure;
using VNQuiz.Alice.Tests.TestsInfrastructure.Models;
using Xunit;
using Xunit.Abstractions;
using Yandex.Alice.Sdk.Models;
using static VNQuiz.Alice.Controllers.AliceController;

namespace VNQuiz.Alice.Tests
{
    public class AliceTests
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly ITestOutputHelper _testOutputHelper;

        public AliceTests(WebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _httpClient = factory.CreateClient();
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Alice_Ping_Success()
        {
            string text = File.ReadAllText(TestsConstants.Assets.AlicePingFilePath);
            HttpContent requestContent = new StringContent(text, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/alice", requestContent);
            string responseContent = await response.Content.ReadAsStringAsync();
            Assert.True(response.IsSuccessStatusCode, responseContent);
            var aliceResponse = JsonSerializer.Deserialize<AliceResponseWrapper>(responseContent);
            Assert.NotNull(aliceResponse);
            _testOutputHelper.WriteLine(aliceResponse.Response.Text);
        }
    }
}
