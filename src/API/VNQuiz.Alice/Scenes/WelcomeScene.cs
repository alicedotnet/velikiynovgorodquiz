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

        protected override SceneType CurrentScene => SceneType.Welcome;

        private readonly string[] _replyVariations = new string[]
        {
            "Предлагаю сыграть в викторину по истории Великого Новгорода. Я буду задавать тебе вопросы, а ты выбирать один из трех вариантов ответа. Игра закончится после трех неправильных ответов. Начнем?",
            "Давай сыграем в викторину по истории Великого Новгорода. Для каждого вопроса будут три варианта ответа. Игра закончится после трех ошибок. Начнем игру?"
        };

        public WelcomeScene(IScenesProvider scenesProvider)
        {
            _scenesProvider = scenesProvider;
        }

        public override Scene? MoveToNextScene(QuizRequest request)
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

        public override QuizResponseBase Reply(QuizRequest request)
        {
            string text = string.Empty;
            if(!request.State.Session.RestorePreviousState)
            {
                text = "Привет!";
            }
            var response = new QuizResponse(
                request,
                text,
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

        public override QuizResponseBase Repeat(QuizRequest request)
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

        //TODO::add possibility to start game from the beginning
        //TODO::think how to show question with additional info at least once per game
        public override QuizResponseBase Help(QuizRequest request)
        {
            return Repeat(request);
        }
    }
}
