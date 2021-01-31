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

        [Theory]        
        [InlineData(TestsConstants.Assets.AlicePingFilePath)]
        [InlineData(TestsConstants.Assets.StartGameFilePath)]
        [InlineData(TestsConstants.Assets.WrongAnswerFilePath)]
        [InlineData(TestsConstants.Assets.LastWrongAnswerFilePath)]
        [InlineData(TestsConstants.Assets.EndGame)]
        [InlineData(TestsConstants.Assets.Question)]
        [InlineData(TestsConstants.Assets.CorrectAnswer)]
        [InlineData(TestsConstants.Assets.AdditionalInfo)]
        [InlineData(TestsConstants.Assets.Rules_GameStarted_Continue)]
        [InlineData(TestsConstants.Assets.Rules_NewSession_StartGame)]
        public async Task Alice_SendRequest_Success(string filePath)
        {
            string text = File.ReadAllText(filePath);
            HttpContent requestContent = new StringContent(text, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/alice", requestContent);
            string responseContent = await response.Content.ReadAsStringAsync();
            Assert.True(response.IsSuccessStatusCode, responseContent);
            var aliceResponse = JsonSerializer.Deserialize<AliceResponseWrapper>(responseContent);
            Assert.NotNull(aliceResponse);
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string prettyJson = JsonSerializer.Serialize(aliceResponse, options);
            _testOutputHelper.WriteLine(prettyJson);
        }
    }
}
