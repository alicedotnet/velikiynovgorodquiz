using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Scenes
{
    public class WelcomeScene : Scene
    {
        private readonly IScenesProvider _scenesProvider;

        protected override string[] FallbackQuestions => new string[]
        {
            "Может сыграем?",
            "Начнем игру?"
        };

        public WelcomeScene(IScenesProvider scenesProvider)
        {
            _scenesProvider = scenesProvider;
        }

        public override Scene MoveToNextScene(QuizRequest request)
        {
            if(request.Session.New)
            {
                return this;
            }
            if (request.Request.Nlu.Intents != null)
            {
                if (request.Request.Nlu.Intents.IsConfirm)
                {
                    return _scenesProvider.Get(SceneType.StartGame);
                }
                else if (request.Request.Nlu.Intents.IsReject)
                {
                    return _scenesProvider.Get(SceneType.EndSession);
                }
            }
            return null;
        }

        public override QuizResponse Reply(QuizRequest request)
        {
            return new QuizResponse(
                request,
                "Привет! Предлагаю сыграть в викторину по истории Великого Новгорода. Я буду задавать тебе вопросы, а ты выбирать один из трех вариантов ответа. Игра закончится после трех неправильных ответов. Начнем?",
                new List<QuizButtonModel>()
                {
                    new QuizButtonModel("да"),
                    new QuizButtonModel("нет")
                })
            {
                SessionState = new QuizSessionState()
            };
        }

        public override QuizResponse Fallback(QuizRequest request)
        {
            var response = base.Fallback(request);
            response.Response.Buttons.Add(new QuizButtonModel("да"));
            response.Response.Buttons.Add(new QuizButtonModel("нет"));
            return response;
        }
    }
}
