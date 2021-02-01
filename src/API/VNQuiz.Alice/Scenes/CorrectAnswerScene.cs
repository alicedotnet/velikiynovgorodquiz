using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;

namespace VNQuiz.Alice.Scenes
{
    public class CorrectAnswerScene : AnswerScene
    {
        protected override string[] AnswerTips { get; }

        protected override SceneType CurrentScene => SceneType.CorrectAnswer;

        public CorrectAnswerScene(IQuestionsService questionsService, IScenesProvider scenesProvider)
            : base(questionsService, scenesProvider)
        {
            AnswerTips = new string[] { "Правильно!", "Верно.", "Вы правы." };
        }

        public override QuizResponseBase Reply(QuizRequest request)
        {
            var response = base.Reply(request);
            response.SessionState.CurrentConsecutiveCorrectAnswers++;
            return response;
        }

        protected override string GetSupportText(QuizSessionState quizSessionState)
        {
            return string.Empty;
        }
    }
}
