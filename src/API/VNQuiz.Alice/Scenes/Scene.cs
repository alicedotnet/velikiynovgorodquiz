using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Scenes
{
    public abstract class Scene
    {
        private readonly string[] _fallbackVariants = new string[] { "Я вас не поняла.", "Не понимаю.", "Не могу разобрать." };
        protected abstract string[] FallbackQuestions { get; }
        protected abstract SceneType CurrentScene { get; }

        public abstract Scene? MoveToNextScene(QuizRequest request);
        public abstract QuizResponseBase Reply(QuizRequest request);

        public abstract QuizResponseBase Repeat(QuizRequest request);

        public abstract QuizResponseBase Help(QuizRequest request);

        public virtual QuizResponseBase Fallback(QuizRequest request)
        {
            return Fallback(request, FallbackQuestions);
        }

        protected virtual void SetFallbackButtons(QuizRequest request, QuizResponseBase response)
        {
            response.Response.Buttons.Add(new QuizButtonModel("да"));
            response.Response.Buttons.Add(new QuizButtonModel("нет"));
        }

        protected QuizResponseBase Fallback(QuizRequest request, string[] fallbackQuestions)
        {
            var response = new QuizResponseBase(request, string.Empty);
            if (request.State.Session.ConsecutiveFallbackAnswers < 1)
            {
                SetRandomSkillAnswer(response, _fallbackVariants);

                var fallbackQuestionsRandom = new Random();
                int fallbackQuestionIndex = fallbackQuestionsRandom.Next(fallbackQuestions.Length);

                string text = JoinString('\n', response.Response.Text, fallbackQuestions[fallbackQuestionIndex]);
                response.Response.SetText(text);
                SetFallbackButtons(request, response);
            }
            else
            {
                response = Repeat(request);
            }
            return response;
        }

        protected void SetRandomSkillAnswer(QuizResponseBase response, string[] values)
        {
            SetRandomSkillAnswer(response, ' ', values);
        }

        protected void SetRandomSkillAnswer(QuizResponseBase response, char separator, string[] values)
        {
            string? value = GetRandomSkillAnswer(response.SessionState, values);
            response.Response.SetText(JoinString(separator, response.Response.Text, value));
        }

        protected string GetRandomSkillAnswer(QuizSessionState sessionState, string[] values)
        {
            string value;
            if (values.Length > 1)
            {
                var random = new Random();
                int index;
                do
                {
                    index = random.Next(values.Length);
                } while (sessionState.LastRandomSkillAnswerIndex != null &&
                    index == sessionState.LastRandomSkillAnswerIndex);
                sessionState.LastRandomSkillAnswerIndex = index;
                value = values[index];
            }
            else
            {
                value = values.First();
            }
            return value;
        }

        protected string GetRandomSkillAnswer(string[] values)
        {
            string value;
            if (values.Length > 1)
            {
                var random = new Random();
                int index = random.Next(values.Length);
                value = values[index];
            }
            else
            {
                value = values.First();
            }
            return value;
        }

        protected string JoinString(char separator, params string?[] values)
        {
            string result = string.Empty;
            foreach (var value in values)
            {
                if(!string.IsNullOrEmpty(value))
                {
                    if(string.IsNullOrEmpty(result))
                    {
                        result = value;
                    }
                    else if(separator == ' ' && result.EndsWith('\n'))
                    {
                        result += value;
                    }
                    else
                    {
                        result = string.Join(separator, result, value);
                    }
                }
            }
            return result;
        }

        protected string GetSentence(string text)
        {
            string sentence = text;
            if (!string.IsNullOrEmpty(sentence))
            {
                char lastSymbol = sentence.Last();
                if (lastSymbol != '.')
                {
                    sentence += '.';
                }
            }
            return sentence;
        }

    }
}
