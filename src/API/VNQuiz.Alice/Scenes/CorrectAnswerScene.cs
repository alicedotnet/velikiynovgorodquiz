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

        protected override string[] FallbackQuestions => Array.Empty<string>();

        public CorrectAnswerScene(IQuestionsService questionsService, IScenesProvider scenesProvider)
            : base(questionsService, scenesProvider)
        {
            AnswerTips = new string[] { "Правильно!" };
        }


        public override QuizResponse Fallback(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override Scene MoveToNextScene(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        protected override string GetSupportText(QuizSessionState quizSessionState)
        {
            return string.Empty;
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
