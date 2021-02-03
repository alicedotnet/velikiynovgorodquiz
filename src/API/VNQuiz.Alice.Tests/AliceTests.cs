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
        [InlineData(TestsConstants.Assets.WinGame)]
        [InlineData(TestsConstants.Assets.Question)]
        [InlineData(TestsConstants.Assets.CorrectAnswer)]
        [InlineData(TestsConstants.Assets.AdditionalInfo)]
        [InlineData(TestsConstants.Assets.Rules_GameStarted_Continue)]
        [InlineData(TestsConstants.Assets.Rules_NewSession_StartGame)]
        [InlineData(TestsConstants.Assets.Fallback)]
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

        [Theory]
        [InlineData("{\"request\":{\"command\":\"князь звали олег\",\"type\":\"SimpleUtterance\",\"original_utterance\":\"князь звали олег\",\"payload\":null,\"markup\":{\"dangerous_context\":false},\"nlu\":{\"tokens\":[\"князь\",\"звали\",\"олег\"],\"entities\":[{\"value\":{\"first_name\":\"олег\",\"last_name\":null},\"tokens\":{\"start\":2,\"end\":3},\"type\":\"YANDEX.FIO\"}],\"intents\":{\"YANDEX.CONFIRM\":null,\"confirm\":null,\"YANDEX.REJECT\":null,\"reject\":null,\"YANDEX.REPEAT\":null,\"repeat\":null,\"YANDEX.HELP\":null,\"help\":null,\"exit\":null,\"YANDEX.WHAT_CAN_YOU_DO\":null,\"rules\":null,\"progress\":null,\"back\":null,\"restart\":null,\"answer\":{\"slots\":{\"number\":null,\"exactAnswer\":{\"value\":\"князь звали олег\",\"tokens\":{\"start\":0,\"end\":3},\"type\":\"YANDEX.STRING\"},\"exactNumber\":null}}}}},\"state\":{\"session\":{\"IncorrectAnswersCount\":0,\"CurrentQuestionId\":3,\"CurrentQuestionAnswers\":[{\"Key\":\"Роман\",\"Value\":\"Роман\"},{\"Key\":\"Рюрик\",\"Value\":\"Рюрик\"},{\"Key\":\"Олег\",\"Value\":\"Олег\"}],\"unansweredQuestionsIds\":[5,7,3,9,2,10,4,6,8,1,11,12],\"CurrentScene\":3,\"NextScene\":0,\"RestorePreviousState\":false,\"LastRandomSkillAnswerIndex\":1,\"ConsecutiveFallbackAnswers\":2,\"UnlockedAchievements\":[],\"IsOpenedAdditionalInfo\":false,\"MaxConsecutiveCorrectAnswers\":0,\"CurrentConsecutiveCorrectAnswers\":0},\"user\":{\"unlockedAchievementsIds\":[1,7],\"AnsweredQuestionsIds\":[5,10,12,8,7,11,9,7]},\"application\":{\"unlockedAchievementsIds\":[],\"AnsweredQuestionsIds\":[]}},\"meta\":{\"locale\":\"ru-RU\",\"timezone\":\"Europe/Minsk\",\"client_id\":\"ru.yandex.mobile/4900 (Apple iPhone; iphone 14.3)\",\"interfaces\":{\"screen\":{},\"payments\":{},\"account_linking\":{}}},\"session\":{\"new\":false,\"session_id\":\"c28924a8-1602-4e75-aca2-92a8045d85b4\",\"message_id\":4,\"skill_id\":\"ae834363-800c-4264-b0f7-70a2a13f32d9\",\"user_id\":\"D8A9FB05C5DBC94E8004AF16B53934E879D75A7D443CAB3F3BD54540BD7D5584\",\"user\":{\"user_id\":\"1A2CFB869ADEB82C0436830DE8ADAC7180921EB180CC1153A10F6596FA0BA1D2\",\"access_token\":null},\"application\":{\"application_id\":\"D8A9FB05C5DBC94E8004AF16B53934E879D75A7D443CAB3F3BD54540BD7D5584\"}},\"version\":\"1.0\"}")]
        public async Task Alice_SendFuzzyAnswerRequest_Success(string text)
        {
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
