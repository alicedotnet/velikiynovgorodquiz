using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Scenes
{
    public abstract class EndGameScene : Scene
    {
        protected override string[] FallbackQuestions => new string[]
        {
            "Может сыграем еще раз?",
            "Повторим игру?"
        };

        private readonly IScenesProvider _scenesProvider;
        protected abstract string[] ReplyVariations { get; }
        protected abstract SceneType CurrentScene { get; }


        public EndGameScene(IScenesProvider scenesProvider)
        {
            _scenesProvider = scenesProvider;
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
            var response = new QuizResponse(request, string.Empty);
            SetRandomSkillAnswer(response, ReplyVariations);
            SetRandomSkillAnswer(response, FallbackQuestions);
            SetFallbackButtons(request, response);
            response.SessionState.CurrentScene = CurrentScene;
            return response;
        }

        protected override void SetFallbackButtons(QuizRequest request, QuizResponse response)
        {
            response.Response.Buttons.Add(new AliceButtonModel("да"));
            response.Response.Buttons.Add(new AliceButtonModel("нет"));
        }

        public override QuizResponse Repeat(QuizRequest request)
        {
            return Reply(request);
        }

        public override QuizResponse Help(QuizRequest request)
        {
            return Reply(request);
        }
    }
}
