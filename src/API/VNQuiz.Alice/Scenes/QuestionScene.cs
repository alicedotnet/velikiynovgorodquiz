using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Helpers;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;
using VNQuiz.Core.Models;
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
        protected override SceneType CurrentScene => SceneType.Question;


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

        public override Scene MoveToNextScene(QuizRequest request)
        {
            int currentQuestionId = request.State.Session.CurrentQuestionId;
            var question = _questionsService.GetQuestion(currentQuestionId);
            string answer = GetAnswer(request);
            if (question.CorrectAnswer == answer)
            {
                return _scenesProvider.Get(SceneType.CorrectAnswer);
            }
            else if (question.WrongAnswers.Contains(answer))
            {
                return _scenesProvider.Get(SceneType.WrongAnswer);
            }
            return null;
        }

        private static string GetAnswer(QuizRequest request)
        {
            if(request.Request.Type == AliceRequestType.ButtonPressed)
            {
                int index = request.Request.GetPayload<int>() - 1;
                return request.State.Session.CurrentQuestionAnswers[index];
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
            var question = _questionsService.GetQuestion(request.State.Session.AnsweredQuestionsIds);
            if(question == null)
            {
                var winGameScene = _scenesProvider.Get(SceneType.WinGame);
                return winGameScene.Reply(request);
            }
            response.Response.SetText($"{response.Response.Text}\n{question.Text}");

            List<string> answers = new List<string>(question.WrongAnswers);
            var random = new Random();
            int correctAnswerIndex = random.Next(0, 2);
            answers.Insert(correctAnswerIndex, question.CorrectAnswer);
            SetAnswers(response, answers.ToArray());

            response.SessionState.CurrentScene = SceneType.Question;
            response.SessionState.CurrentQuestionId = question.Id;
            response.SessionState.CurrentQuestionAnswers = answers.ToArray();
            return response;
        }

        private static void SetAnswersFromSession(QuizRequest request, QuizResponse response)
        {
            SetAnswers(response, request.State.Session.CurrentQuestionAnswers);
        }


        private static void SetAnswers(QuizResponse response, string[] answers)
        {
            for (int i = 1; i <= answers.Length; i++)
            {
                response.Response.Buttons.Add(new QuizButtonModel(i.ToString(), i));
                response.Response.AppendText($"\n{EmojiHelper.GetNumberEmoji(i)} {answers[i - 1]}", false);
                response.Response.AppendTts($"\n{answers[i - 1]}");
            }
        }

        public override QuizResponse Repeat(QuizRequest request)
        {
            var response = new QuizResponse(
                request,
                string.Empty);
            SetRandomSkillAnswer(response, _repeatVariations);
            var question = _questionsService.GetQuestion(request.State.Session.CurrentQuestionId);
            response.Response.SetText(response.Response.Text + "\n" + question.Text);
            SetAnswersFromSession(request, response);
            return response;
        }

        public override QuizResponse Help(QuizRequest request)
        {
            return Repeat(request);
        }
    }
}
