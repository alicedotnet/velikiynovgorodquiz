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

        private readonly IScenesProvider _scenesProvider;

        public StartGameScene(IScenesProvider scenesProvider)
        {
            _scenesProvider = scenesProvider;
        }

        public override QuizResponse Fallback(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override Scene MoveToNextScene(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override QuizResponse Reply(QuizRequest request)
        {
            var questionScene = _scenesProvider.Get(SceneType.Question);
            request.State.Session.AnsweredQuestionsIds.Clear();
            request.State.Session.CurrentQuestionId = 0;
            request.State.Session.IncorrectAnswersCount = 0;
            return questionScene.Reply(request);
        }
    }
}
