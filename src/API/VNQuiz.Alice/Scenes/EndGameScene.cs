using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Scenes
{
    public class EndGameScene : Scene
    {
        protected override string[] FallbackQuestions => Array.Empty<string>();

        private readonly IScenesProvider _scenesProvider;

        public EndGameScene(IScenesProvider scenesProvider)
        {
            _scenesProvider = scenesProvider;
        }

        public override QuizResponse Fallback(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override Scene MoveToNextScene(QuizRequest request)
        {
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
            var response = new QuizResponse(request, "К сожалению, наша игра подошла к концу. Сыграем еще раз?");
            response.Response.Buttons.Add(new AliceButtonModel("да"));
            response.Response.Buttons.Add(new AliceButtonModel("нет"));
            response.SessionState.CurrentScene = SceneType.EndGame;
            return response;
        }

        public override QuizResponse Repeat(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override QuizResponse Help(QuizRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
