using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;

namespace VNQuiz.Alice.Scenes
{
    public class RequestRestartScene : Scene
    {
        private readonly IScenesProvider _scenesProvider;

        public RequestRestartScene(IScenesProvider scenesProvider)
        {
            _scenesProvider = scenesProvider;
        }

        protected override string[] FallbackQuestions => new string[]
        {
            "Вы действительно хотите начать игру заново?",
            "Вы и правда хотите начать заново?"
        };

        protected override SceneType CurrentScene => SceneType.RequestRestart;

        private readonly SceneType[] _excludePreviousStateRestore = Array.Empty<SceneType>();

        public override QuizResponseBase Help(QuizRequest request)
        {
            return Reply(request);
        }

        public override Scene? MoveToNextScene(QuizRequest request)
        {
            if(request.Request.Nlu.Intents != null)
            {
                if(request.Request.Nlu.Intents.IsConfirm)
                {
                    return _scenesProvider.Get(SceneType.StartGame);
                }
                else if(request.Request.Nlu.Intents.IsReject)
                {
                    return _scenesProvider.Get(request.State.Session.NextScene);
                }
            }
            return null;
        }

        public override QuizResponseBase Repeat(QuizRequest request)
        {
            return Reply(request);
        }

        public override QuizResponseBase Reply(QuizRequest request)
        {
            if(request.State.Session.CurrentScene > SceneType.StartGame 
                && request.State.Session.CurrentScene <= SceneType.RulesScene
                && request.State.Session.CurrentScene != CurrentScene)
            {
                var response = new QuizResponse(request, string.Empty);
                SetRandomSkillAnswer(response, '\n', FallbackQuestions);
                SetFallbackButtons(request, response);
                if(!_excludePreviousStateRestore.Contains(response.SessionState.CurrentScene))
                {
                    response.SessionState.RestorePreviousState = true;
                }

                if(request.State.Session.CurrentScene != CurrentScene
                    && request.State.Session.CurrentScene != SceneType.RulesScene
                    && request.State.Session.CurrentScene != SceneType.ProgressScene)
                {
                    if (request.State.Session.CurrentScene == SceneType.AdditionalInfo)
                    {
                        request.State.Session.NextScene = SceneType.Question;
                    }
                    else
                    {
                        request.State.Session.NextScene = request.State.Session.CurrentScene;
                    }
                }
                response.SessionState.CurrentScene = CurrentScene;
                return response;
            }
            var startGameScene = _scenesProvider.Get(SceneType.StartGame);
            return startGameScene.Reply(request);
        }
    }
}
