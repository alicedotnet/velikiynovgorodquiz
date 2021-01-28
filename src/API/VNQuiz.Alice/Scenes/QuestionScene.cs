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
        protected override string[] FallbackQuestions => new string[]
        {
            "Попробуй еще раз",
            "Какой будет твой ответ на вопрос?"
        };

        private readonly string[] _firstQuestionAnswers = new string[]
        {
            "Отлично, начинаем! Вот первый вопрос:",
            "Начинаем игру. Первый вопрос:"
        };

        private readonly string[] _nextQuestionAnswers = new string[]
        {
            "Вот следующий вопрос:",
            "Идем дальше.",
            "Следующий вопрос:"
        };

        private readonly string[] _repeatVariations = new string[]
        {
            "Повторяю вопрос:",
            "Вот вопрос:"
        };


        private readonly IQuestionsService _questionsService;
        private readonly IScenesProvider _scenesProvider;

        public QuestionScene(IQuestionsService questionsService, IScenesProvider scenesProvider)
        {
            _questionsService = questionsService;
            _scenesProvider = scenesProvider;
        }

        protected override void SetFallbackButtons(QuizRequest request, QuizResponse response)
        {
            SetAnswersFromSession(request, response);
        }

        private void SetAnswersFromSession(QuizRequest request, QuizResponse response)
        {
            foreach (string currentQuestionAnswer in request.State.Session.CurrentQuestionAnswers)
            {
                response.Response.Buttons.Add(new QuizButtonModel(currentQuestionAnswer));
            }
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
            if(request.Request.Nlu.Intents?.Answer != null)
            {
                if(request.Request.Nlu.Intents.Answer.Slots.Number != null)
                {
                    int index = (int)request.Request.Nlu.Intents.Answer.Slots.Number.Value - 1;
                    return request.State.Session.CurrentQuestionAnswers[index];
                }
                if (request.Request.Nlu.Intents.Answer.Slots.ExactAnswer != null)
                {
                    return request.Request.Nlu.Intents.Answer.Slots.ExactAnswer.Value;
                }
            }
            return request.Request.Command;
        }

        public override QuizResponse Reply(QuizRequest request)
        {
            var response = new QuizResponse(request, string.Empty);
            if(response.SessionState.CurrentQuestionId == 0)
            {
                SetRandomSkillAnswer(response, _firstQuestionAnswers);
            }
            else
            {
                SetRandomSkillAnswer(response, _nextQuestionAnswers);
            }
            var question = _questionsService.GetQuestion();
            response.Response.Text = $"{response.Response.Text}\n\n{question.Text}";
            foreach (var wrongAnswer in question.WrongAnswers)
            {
                response.Response.Buttons.Add(new QuizButtonModel(wrongAnswer));
            }
            var random = new Random();
            int correctAnswerIndex = random.Next(0, 2);
            response.Response.Buttons.Insert(correctAnswerIndex, new QuizButtonModel(question.CorrectAnswer));
            response.SessionState.CurrentScene = SceneType.Question;
            response.SessionState.CurrentQuestionId = question.Id;
            response.SessionState.CurrentQuestionAnswers = response.Response.Buttons.Select(x => x.Title).ToArray();
            return response;
        }

        public override QuizResponse Repeat(QuizRequest request)
        {
            var response = new QuizResponse(
                request,
                string.Empty);
            SetRandomSkillAnswer(response, _repeatVariations);
            var question = _questionsService.GetQuestion(request.State.Session.CurrentQuestionId);
            response.Response.Text += "\n" + question.Text;
            SetAnswersFromSession(request, response);
            return response;
        }

        public override QuizResponse Help(QuizRequest request)
        {
            return Repeat(request);
        }
    }
}
