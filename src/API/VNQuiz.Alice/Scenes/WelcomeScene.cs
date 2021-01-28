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

        private readonly string[] _replyVariations = new string[]
        {
            "Предлагаю сыграть в викторину по истории Великого Новгорода. Я буду задавать тебе вопросы, а ты выбирать один из трех вариантов ответа. Игра закончится после трех неправильных ответов. Начнем?",
            "Давай сыграем в викторину по истории Великого Новгорода. Для каждого вопроса будут три варианта ответа. Игра закончится после трех ошибок. Начнем игру?"
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
            var response = new QuizResponse(
                request,
                "Привет!",
                new List<QuizButtonModel>()
                {
                    new QuizButtonModel("да"),
                    new QuizButtonModel("нет")
                })
            {
                SessionState = new QuizSessionState()
            };
            SetRandomSkillAnswer(response, _replyVariations);
            return response;
        }

        protected override void SetFallbackButtons(QuizRequest request, QuizResponse response)
        {
            response.Response.Buttons.Add(new QuizButtonModel("да"));
            response.Response.Buttons.Add(new QuizButtonModel("нет"));
        }

        public override QuizResponse Repeat(QuizRequest request)
        {
            var response = new QuizResponse(
                request,
                string.Empty,
                new List<QuizButtonModel>()
                {
                    new QuizButtonModel("да"),
                    new QuizButtonModel("нет")
                });
            SetRandomSkillAnswer(response, _replyVariations);
            return response;
        }

        //TODO::probably need to have separate screen for rules
        //TODO::add possibility to end session at any time
        public override QuizResponse Help(QuizRequest request)
        {
            return Repeat(request);
        }
    }
}
