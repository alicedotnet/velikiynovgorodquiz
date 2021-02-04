using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;

namespace VNQuiz.Alice.Scenes
{
    public class RulesScene : Scene
    {
        protected override string[] FallbackQuestions => Array.Empty<string>();

        protected override SceneType CurrentScene => SceneType.RulesScene;

        private readonly string[] _replyVariations = new string[]
        {
            "Игра-викторина по истории Великого Новгорода. Я буду задавать вопросы, а вы выбирать один из трех вариантов ответа. Игра закончится после трех неправильных ответов.",
            "Играем викторину по истории Великого Новгорода. Для каждого вопроса будут три варианта ответа. Игра закончится после трех ошибок."
        };

        private readonly string[] _gameNotStartedQuestions = new string[]
        {
            "Сыграем?",
            "Начнем игру?"
        };

        private readonly string[] _gameStartedQuestions = new string[]
        {
            "Продолжаем?",
            "Продолжим?"
        };
        private readonly IScenesProvider _scenesProvider;

        public RulesScene(IScenesProvider scenesProvider)
        {
            _scenesProvider = scenesProvider;
        }

        public override QuizResponseBase Help(QuizRequest request)
        {
            return Reply(request);
        }

        public override Scene? MoveToNextScene(QuizRequest request)
        {
            if(request.Request.Nlu.Intents != null)
            {
                if(request.Request.Nlu.Intents.IsConfirm || request.Request.Nlu.Intents.IsNext)
                {
                    if (request.State.Session.NextScene <= SceneType.Welcome)
                    {
                        request.State.Session.RestorePreviousState = false;
                        return _scenesProvider.Get(SceneType.StartGame);
                    }
                    return _scenesProvider.Get(request.State.Session.NextScene);
                }
                else if(request.Request.Nlu.Intents.IsReject)
                {
                    if (request.State.Session.NextScene <= SceneType.Welcome
                        || request.State.Session.NextScene > SceneType.RulesScene)
                    {
                        return _scenesProvider.Get(SceneType.EndSession);
                    }
                    return _scenesProvider.Get(SceneType.RequestEndSession);
                }
            }
            return null;
        }

        public override QuizResponseBase Repeat(QuizRequest request)
        {
            return Reply(request);
        }

        public override QuizResponseBase Fallback(QuizRequest request)
        {
            QuizResponseBase response;
            if (request.State.Session.NextScene <= SceneType.Welcome)
            {
                response = Fallback(request, _gameNotStartedQuestions);
            }
            else
            {
                response = Fallback(request, _gameStartedQuestions);
            }
            return response;
        }

        public override QuizResponseBase Reply(QuizRequest request)
        {
            if(request.State.Session.CurrentScene != CurrentScene
                && request.State.Session.CurrentScene != SceneType.RequestEndSession)
            {
                request.State.Session.NextScene = request.State.Session.CurrentScene;
            }
            var response = new QuizResponse(request, string.Empty);
            SetRandomSkillAnswer(response, _replyVariations);
            if (request.State.Session.NextScene <= SceneType.Welcome)
            {
                SetRandomSkillAnswer(response, _gameNotStartedQuestions);
            }
            else
            {
                SetRandomSkillAnswer(response, _gameStartedQuestions);
            }
            SetFallbackButtons(request, response);
            response.SessionState.CurrentScene = CurrentScene;
            response.SessionState.RestorePreviousState = true;
            return response;
        }
    }
}
