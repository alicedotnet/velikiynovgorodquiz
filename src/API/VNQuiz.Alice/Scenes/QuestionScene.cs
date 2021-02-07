using FuzzySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Helpers;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;
using VNQuiz.Core.Models;
using Yandex.Alice.Sdk.Helpers;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Scenes
{
    public class QuestionScene : Scene
    {
        protected override string[] FallbackQuestions => new string[]
        {
            "Попробуй еще раз",
            "Какой будет ваш ответ на вопрос?"
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
            "Продолжаем.",
            "Следующий вопрос:"
        };

        private readonly string[] _repeatVariations = new string[]
        {
            "Повторяю вопрос:",
            "Вот вопрос:"
        };

        private readonly string[] _needMoreInfoVariations = new string[]
        {
            "Уточните, пожалуйста.",
            "Уточните, пожалуйста, ответ."
        };

        private bool _isNeedMoreInfo = false;


        private readonly IQuestionsService _questionsService;
        private readonly IScenesProvider _scenesProvider;

        public QuestionScene(IQuestionsService questionsService, IScenesProvider scenesProvider)
        {
            _questionsService = questionsService;
            _scenesProvider = scenesProvider;
        }

        protected override void SetFallbackButtons(QuizRequest request, QuizResponseBase response)
        {
            SetAnswersFromSession(request, response);
        }

        public override Scene? MoveToNextScene(QuizRequest request)
        {
            int currentQuestionId = request.State.Session.CurrentQuestionId;
            var question = _questionsService.GetQuestion(currentQuestionId);
            bool? isCorrectAnswer = IsCorrectAnswer(request, question);
            if(isCorrectAnswer != null)
            {
                if (isCorrectAnswer.Value)
                {
                    return _scenesProvider.Get(SceneType.CorrectAnswer);
                }
                else
                {
                    return _scenesProvider.Get(SceneType.WrongAnswer);
                }
            } else if(_isNeedMoreInfo)
            {
                return this;
            }
            return null;
        }

        private readonly string[] _excludeWords = Array.Empty<string>();

        public bool? IsCorrectAnswer(QuizRequest request, Question question)
        {
            _isNeedMoreInfo = false;
            string answer = GetAnswerText(request);
            answer = Preprocess(answer)!;
            string? correctAnswer = Preprocess(question.CorrectAnswer);
            if(answer == correctAnswer)
            {
                return true;
            }

            List<string?> preProcessedAnswers = new List<string?>()
            {
                correctAnswer
            };

            foreach (string wrongAnswer in question.WrongAnswers)
            {
                string? preProcessedWrongAnswer = Preprocess(wrongAnswer);
                if(answer == preProcessedWrongAnswer)
                {
                    return false;
                }
                preProcessedAnswers.Add(preProcessedWrongAnswer);
            }

            var wordsInAnswers = new List<string>();
            foreach (var preProcessedAnswer in preProcessedAnswers)
            {
                wordsInAnswers.AddRange(preProcessedAnswer!.Split(' '));
            }

            string[] inputParts = answer.Split(' ');
            var remainingParts = new List<string>();

            foreach (string? inputPart in inputParts)
            {
                if ((FuzzyContains(inputPart, question.Text.ToLower().Split(' ', ','))
                        || FuzzyContains(inputPart, _excludeWords))
                    && !FuzzyContains(inputPart, wordsInAnswers))
                {
                    continue;
                }
                remainingParts.Add(inputPart);
            }
            string postProcessedInput = string.Join(' ', remainingParts);

            var answersMatches = new Dictionary<int, bool>();
            for (int i = 0; i < preProcessedAnswers.Count; i++)
            {
                answersMatches.Add(i, IsAnswerMatch(postProcessedInput, preProcessedAnswers[i]!));
            }

            int matchedAnswersCount = answersMatches.Values.Count(x => x == true);
            if (matchedAnswersCount == 1)
            {
                var answerMatch = answersMatches.First(x => x.Value == true);
                if(answerMatch.Key == 0)
                {
                    return true;
                }
                return false;
            }
            else if(matchedAnswersCount > 1)
            {
                _isNeedMoreInfo = true;
            }

            return null;
        }

        private static bool IsAnswerMatch(string userValue, string answer)
        {
            int similarity = Fuzz.Ratio(userValue, answer);
            if (similarity >= 75)
            {
                return true;
            }

            var userWords = userValue.Split(' ');
            if(userWords.Length > 0 && userWords.Length <= 2)
            {
                var answerWords = answer.Split(' ', '-');
                foreach (var userWord in userWords)
                {
                    if(!FuzzyContains(userWord, answerWords))
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }

        private static bool FuzzyContains(string value, IEnumerable<string> collection)
        {
            foreach (var item in collection)
            {
                int similarity = Fuzz.Ratio(value, item);
                if((similarity >= 75 && item.Length == 4) || similarity >= 80)
                {
                    return true;
                }
            }
            return false;
        }

        private static string? Preprocess(string? value)
        {
            return PrepareText(value)?.ToLower();
        }

        private static string? PrepareText(string? value)
        {
            if (value == null)
            {
                return null;
            }
            value = value.Replace("+", string.Empty);
            if (value.EndsWith("-м"))
            {
                value = value.TrimEnd('м').TrimEnd('-');
            }
            return value;
        }

        private static string GetAnswerText(QuizRequest request)
        {
            if(request.Request.Type == AliceRequestType.ButtonPressed)
            {
                int index = request.Request.GetPayload<int>() - 1;
                return request.State.Session.CurrentQuestionAnswers.ElementAt(index);
            }
            if(request.Request.Nlu.Intents?.Answer != null)
            {
                if(request.Request.Nlu.Intents.Answer.Slots.Number != null)
                {
                    int index = (int)request.Request.Nlu.Intents.Answer.Slots.Number!.Value - 1;
                    return request.State.Session.CurrentQuestionAnswers.ElementAt(index);
                }
                if(request.Request.Nlu.Intents.Answer.Slots.ExactNumber != null)
                {
                    return request.Request.Nlu.Intents.Answer.Slots.ExactNumber!.Value.ToString();
                }
                if (request.Request.Nlu.Intents.Answer.Slots.ExactAnswer != null)
                {
                    return request.Request.Nlu.Intents.Answer.Slots.ExactAnswer!.Value;
                }
            }
            return request.Request.Command;
        }

        public override QuizResponseBase Reply(QuizRequest request)
        {
            if(_isNeedMoreInfo)
            {
                return NeedMoreInfo(request);
            }

            Question? question;
            if (request.State.Session.RestorePreviousState)
            {
                question = _questionsService.GetQuestion(request.State.Session.CurrentQuestionId);
            }
            else
            {
                question = _questionsService.GetQuestion(request);
                if (question == null)
                {
                    var winGameScene = _scenesProvider.Get(SceneType.WinGame);
                    return winGameScene.Reply(request);
                }
            }

            string? text = string.Empty;
            if(!request.State.Session.RestorePreviousState)
            {
                if (request.State.Session.CurrentQuestionId == 0)
                {
                    text = GetRandomSkillAnswer(_firstQuestionAnswers);
                }
                else
                {
                    text = GetRandomSkillAnswer(_nextQuestionAnswers);
                }
            }
            request.State.Session.RestorePreviousState = false;

            var response = new QuizResponseBase(request, text);
            response.Response.SetText(
                JoinString('\n', response.Response.Text, question.Text) + AliceHelper.SilenceString500);

            var answers = new List<string>();
            foreach (string? wrongAnswer in question.WrongAnswers)
            {
                answers.Add(wrongAnswer);
            }
            var random = new Random();
            int correctAnswerIndex = question.Shuffle.GetValueOrDefault(true) ? random.Next(0, 2) : 0;
            answers.Insert(correctAnswerIndex, question.CorrectAnswer);
            SetAnswers(response, answers);

            response.SessionState.CurrentScene = SceneType.Question;
            response.SessionState.CurrentQuestionId = question.Id;
            response.SessionState.CurrentQuestionAnswers = answers;
            return response;
        }

        private static void SetAnswersFromSession(QuizRequest request, QuizResponseBase response)
        {
            SetAnswers(response, request.State.Session.CurrentQuestionAnswers);
        }



        private static void SetAnswers(QuizResponseBase response, List<string> answers)
        {
            for (int i = 1; i <= answers.Count; i++)
            {
                response.Response.Buttons.Add(new QuizButtonModel(i.ToString(), i));
                response.Response.AppendText($"\n{i}. {PrepareText(answers[i - 1])}", false);
                response.Response.AppendTts($"\n{answers[i - 1]}{AliceHelper.SilenceString500}");
            }
        }

        public QuizResponseBase NeedMoreInfo(QuizRequest request)
        {
            _isNeedMoreInfo = false;
            var response = new QuizResponse(
                request,
                string.Empty);
            SetRandomSkillAnswer(response, _needMoreInfoVariations);
            SetAnswersFromSession(request, response);
            return response;
        }

        public override QuizResponseBase Repeat(QuizRequest request)
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

        public override QuizResponseBase Help(QuizRequest request)
        {
            return Repeat(request);
        }
    }
}
