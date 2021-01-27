using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Scenes
{
    public class QuestionScene : Scene
    {
        protected override string[] FallbackQuestions => Array.Empty<string>();

        private readonly IQuestionsService _questionsService;
        private readonly IScenesProvider _scenesProvider;

        public QuestionScene(IQuestionsService questionsService, IScenesProvider scenesProvider)
        {
            _questionsService = questionsService;
            _scenesProvider = scenesProvider;
        }

        public override QuizResponse Fallback(QuizRequest request)
        {
            return new QuizResponse(request, "Я тебя не поняла. Повтори еще раз");
        }

        public override Scene MoveToNextScene(QuizRequest request)
        {
            int currentQuestionId = request.State.Session.CurrentQuestionId;
            var question = _questionsService.GetQuestion(currentQuestionId);
            if (question.CorrectAnswer == GetAnswer(request))
            {
                return _scenesProvider.Get(SceneType.CorrectAnswer);
            }
            else if (question.WrongAnswers.Contains(GetAnswer(request)))
            {
                return _scenesProvider.Get(SceneType.WrongAnswer);
            }
            return null;
        }

        private static string GetAnswer(QuizRequest request)
        {
            if(request.Request.Type == AliceRequestType.ButtonPressed)
            {
                return request.Request.Payload.ToString();
            }
            return request.Request.Command;
        }

        public override QuizResponse Reply(QuizRequest request)
        {
            var question = _questionsService.GetQuestion();
            var response = new QuizResponse(request, question.Text);
            string questionTipText;
            if(response.SessionState.CurrentQuestionId == 0)
            {
                questionTipText = "Отлично, начинаем! Вот первый вопрос:\n";
            }
            else
            {
                questionTipText = "Вот следующий вопрос:";
            }
            response.Response.Text = $"{questionTipText}\n{response.Response.Text}";
            foreach (var wrongAnswer in question.WrongAnswers)
            {
                response.Response.Buttons.Add(new QuizButtonModel(wrongAnswer));
            }
            var random = new Random();
            int correctAnswerIndex = random.Next(0, 2);
            response.Response.Buttons.Insert(correctAnswerIndex, new QuizButtonModel(question.CorrectAnswer));
            response.SessionState.CurrentScene = SceneType.Question;
            response.SessionState.CurrentQuestionId = question.Id;
            return response;
        }
    }
}
