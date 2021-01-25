using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;
using Xunit;
using Xunit.Abstractions;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Tests.Services
{
    public class AliceServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public AliceServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ProcessNewSession_StartNewGameYes_Success()
        {
            var questionsServiceMock = new Mock<QuestionsService>();
            IAliceService aliceService = new AliceService(questionsServiceMock.Object);
            var request = new AliceQuizRequest
            {
                Session = new AliceSessionModel(),
                State = new AliceStateModel<QuizSessionState, object>()
                {
                    Session = new QuizSessionState()
                    {
                        QuizState = QuizState.GameNotStarted
                    }
                },
                Request = new AliceRequestModel<AliceQuizIntentModel>()
                {
                    Command = "да"
                }
            };

            var response = aliceService.ProcessNewSession(request);
            Assert.NotNull(response);
            string responseText = JsonSerializer.Serialize(response, new JsonSerializerOptions() { WriteIndented = true });
            _testOutputHelper.WriteLine(responseText);
        }
    }
}
