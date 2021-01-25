using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Services
{
    public class AliceService : IAliceService
    {
        private readonly IQuestionsService _questionsService;

        public AliceService(IQuestionsService questionsService)
        {
            _questionsService = questionsService;
        }

        public AliceQuizResponse ProcessNewSession(AliceQuizRequest request)
        {
            if (request.Session.New)
            {
                return new AliceQuizResponse(
                    request,
                    "Привет! Предлагаю сыграть в викторину по истории Великого Новгорода. Я буду задавать тебе вопросы, а ты выбирать один из трех вариантов ответа. Игра закончится после трех неправильных ответов. Начнем?",
                    new List<AliceButtonModel>()
                    {
                        new AliceButtonModel("да"),
                        new AliceButtonModel("нет")
                    })
                {
                    SessionState = new QuizSessionState()
                };
            }
            else if (request.State.Session.QuizState == QuizState.GameNotStarted)
            {
                if(request.Request.Nlu.Intents != null)
                {
                    if (request.Request.Nlu.Intents.YandexConfirm != null
                        || request.Request.Nlu.Intents.Confirm != null)
                    {
                        var question = _questionsService.GetQuestion();
                        var response = new AliceQuizResponse(request, question.Text);
                        foreach (var answer in question.Answers)
                        {
                            response.Response.Buttons.Add(new AliceButtonModel(answer));
                        }
                        response.SessionState.QuizState = QuizState.GameStarted;
                        return response;
                    }
                    else if (request.Request.Nlu.Intents.YandexReject != null
                        || request.Request.Nlu.Intents.Reject != null)
                    {
                        var response = new AliceQuizResponse(
                            request,
                            "Очень жаль это слышать. Буду ждать еще!");
                        response.Response.EndSession = true;
                        return response;
                    }
                }
            }
            return new AliceQuizResponse(request, "Я вас не поняла. Может сыграем?", new List<AliceButtonModel>
            {
                new AliceButtonModel("да"),
                new AliceButtonModel("нет")
            });
        }
    }
}
