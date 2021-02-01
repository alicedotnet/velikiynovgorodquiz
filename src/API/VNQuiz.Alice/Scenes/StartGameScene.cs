using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;

namespace VNQuiz.Alice.Scenes
{
    public class StartGameScene : Scene
    {
        protected override string[] FallbackQuestions => Array.Empty<string>();
        protected override SceneType CurrentScene => SceneType.StartGame;

        private readonly IScenesProvider _scenesProvider;
        private readonly IQuestionsService _questionsService;

        public StartGameScene(IScenesProvider scenesProvider, IQuestionsService questionsService)
        {
            _scenesProvider = scenesProvider;
            _questionsService = questionsService;
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
            request.State.Session.MaxConsecutiveCorrectAnswers = 0;
            request.State.Session.CurrentConsecutiveCorrectAnswers = 0;
            request.State.Session.RestorePreviousState = false;
            request.State.Session.UnansweredQuestionsIds = _questionsService.GetQuestionsIds();
            request.State.Session.IsOpenedAdditionalInfo = false;
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
