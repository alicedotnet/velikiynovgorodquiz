using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Scenes;
using VNQuiz.Alice.Tests.TestsInfrastructure;
using VNQuiz.Alice.Tests.TestsInfrastructure.Fixtures;
using VNQuiz.Alice.Tests.TestsInfrastructure.Models;
using Xunit;
using Xunit.Abstractions;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Tests
{
    public class AliceTests
        : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly ITestOutputHelper _testOutputHelper;

        public AliceTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _httpClient = factory.CreateClient();
            _testOutputHelper = testOutputHelper;
        }

        [Theory]        
        [InlineData(TestsConstants.Assets.AlicePingFilePath)]
        [InlineData(TestsConstants.Assets.AllAchievementsUnlockedFilePath)]
        [InlineData(TestsConstants.Assets.AnswerNeedMoreInfo)]
        [InlineData(TestsConstants.Assets.StartGameFilePath)]
        [InlineData(TestsConstants.Assets.WrongAnswerFilePath)]
        [InlineData(TestsConstants.Assets.LastWrongAnswerFilePath)]
        [InlineData(TestsConstants.Assets.LastWrongAnswerAchievementUnlockedFilePath)]
        [InlineData(TestsConstants.Assets.LoseGame_StartNew)]
        [InlineData(TestsConstants.Assets.WinGame)]
        [InlineData(TestsConstants.Assets.Question)]
        [InlineData(TestsConstants.Assets.Question_AdditionalInfo)]
        [InlineData(TestsConstants.Assets.CorrectAnswer)]
        [InlineData(TestsConstants.Assets.AdditionalInfo)]
        [InlineData(TestsConstants.Assets.Rules_GameStarted_Continue)]
        [InlineData(TestsConstants.Assets.Rules_NewSession_StartGame)]
        [InlineData(TestsConstants.Assets.Fallback)]
        [InlineData(TestsConstants.Assets.Progress)]
        public async Task Alice_SendRequest_Success(string filePath)
        {
            string text = File.ReadAllText(filePath, Encoding.Default);
            HttpContent requestContent = new StringContent(text, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/alice", requestContent);
            string responseContent = await response.Content.ReadAsStringAsync();
            Assert.True(response.IsSuccessStatusCode, responseContent);
            var aliceResponse = JsonSerializer.Deserialize<QuizResponseBase>(responseContent);
            Assert.NotNull(aliceResponse);
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string prettyJson = JsonSerializer.Serialize(aliceResponse, options);
            _testOutputHelper.WriteLine(prettyJson);
        }

        [Fact]
        public async Task Alice_GetSpecificQuestionId_Success()
        {
            var request = new QuizRequest()
            {
                Request = new AliceRequestModel<QuizIntentModel>()
                {
                    OriginalUtterance = "/question_44",
                },
                State = new AliceStateModel<QuizSessionState, QuizUserState>
                {
                    Session = new QuizSessionState()
                }
            };
            string text = JsonSerializer.Serialize(request);
            HttpContent requestContent = new StringContent(text, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/alice", requestContent);
            string responseContent = await response.Content.ReadAsStringAsync();
            Assert.True(response.IsSuccessStatusCode, responseContent);
            var aliceResponse = JsonSerializer.Deserialize<QuizResponseBase>(responseContent);
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string prettyJson = JsonSerializer.Serialize(aliceResponse, options);
            _testOutputHelper.WriteLine(prettyJson);
        }


        [Fact]
        public async Task Alice_QuestionScene_SkipQuestion()
        {
            var request = new QuizRequest()
            {
                Request = new AliceRequestModel<QuizIntentModel>()
                {
                    Nlu = new AliceNLUModel<QuizIntentModel>()
                    {
                        Intents = new QuizIntentModel()
                        {
                            Skip = new AliceIntentModel()
                        }
                    }
                },
                State = new AliceStateModel<QuizSessionState, QuizUserState>()
                {
                    Session = new QuizSessionState()
                    {
                        CurrentScene = SceneType.Question,
                        CurrentQuestionId = 1,
                        UnansweredQuestionsIds = new List<int>() { 2 }
                    },
                    User = new QuizUserState()
                    {
                        AnsweredQuestionsIds = new Queue<int>()
                    }
                }
            };
            request.State.User.AnsweredQuestionsIds.Enqueue(3);
            string text = JsonSerializer.Serialize(request);
            HttpContent requestContent = new StringContent(text, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/alice", requestContent);
            string responseContent = await response.Content.ReadAsStringAsync();
            Assert.True(response.IsSuccessStatusCode, responseContent);
            var aliceResponse = JsonSerializer.Deserialize<QuizResponseBase>(responseContent);
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
