using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;

namespace VNQuiz.Alice.Scenes
{
    public class StartGameScene : Scene
    {
        protected override string[] FallbackQuestions => Array.Empty<string>();
        protected override SceneType CurrentScene => SceneType.StartGame;

        private readonly IScenesProvider _scenesProvider;

        public StartGameScene(IScenesProvider scenesProvider)
        {
            _scenesProvider = scenesProvider;
        }

        public override QuizResponseBase Fallback(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override Scene MoveToNextScene(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override QuizResponseBase Reply(QuizRequest request)
        {
            var questionScene = _scenesProvider.Get(SceneType.Question);
            request.State.Session.UnlockedAchievements.Clear();
            request.State.Session.CurrentQuestionId = 0;
            request.State.Session.IncorrectAnswersCount = 0;
            return questionScene.Reply(request);
        }

        public override QuizResponseBase Repeat(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override QuizResponseBase Help(QuizRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
