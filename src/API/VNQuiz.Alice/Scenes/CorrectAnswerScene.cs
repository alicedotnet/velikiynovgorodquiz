using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;

namespace VNQuiz.Alice.Scenes
{
    public class CorrectAnswerScene : Scene
    {
        private readonly IQuestionsService _questionsService;

        public CorrectAnswerScene(IQuestionsService questionsService)
        {
            _questionsService = questionsService;
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
            var question = _questionsService.GetQuestion(request.State.Session.CurrentQuestionId);
            string text = "Правильно! " + question.Explanation;
            var response = new QuizResponse(request, text);
            response.SessionState.AnsweredQuestionsIds.Add(question.QuestionId);
            return response;
        }
    }
}
