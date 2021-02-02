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

        private readonly string[] _supportTexts = new string[]
        {
            string.Empty,
            "Так держать!",
            string.Empty,
            "Вы молодец.",
            string.Empty,
            string.Empty,
            "У вас отлично получается!",
            string.Empty,
        };

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
            return GetRandomSkillAnswer(_supportTexts);
        }
    }
}
