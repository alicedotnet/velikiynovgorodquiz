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
                new List<AliceButtonModel>()
                {
                    new AliceButtonModel("да"),
                    new AliceButtonModel("нет")
                })
            {
                SessionState = new QuizSessionState()
            };
        }

        public override QuizResponse Fallback(QuizRequest request)
        {
            return new QuizResponse(request, "Я вас не поняла. Может сыграем?", new List<AliceButtonModel>
            {
                new AliceButtonModel("да"),
                new AliceButtonModel("нет")
            });
        }
    }
}
