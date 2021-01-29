﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;

namespace VNQuiz.Alice.Scenes
{
    public abstract class Scene
    {
        private readonly string[] _fallbackVariants = new string[] { "Я вас не поняла.", "Не понимаю.", "Не могу разобрать." };
        protected abstract string[] FallbackQuestions { get; }
        protected abstract SceneType CurrentScene { get; }

        public abstract Scene MoveToNextScene(QuizRequest request);
        public abstract QuizResponse Reply(QuizRequest request);

        public abstract QuizResponse Repeat(QuizRequest request);

        public abstract QuizResponse Help(QuizRequest request);

        public virtual QuizResponse Fallback(QuizRequest request)
        {
            QuizResponse response;
            if (request.State.Session.ConsecutiveFallbackAnswers < 1)
            {
                response = Fallback(request, FallbackQuestions);
                SetFallbackButtons(request, response);
            }
            else
            {
                response = Repeat(request);
            }
            return response;
        }

        protected virtual void SetFallbackButtons(QuizRequest request, QuizResponse response)
        {

        }

        protected QuizResponse Fallback(QuizRequest request, string[] fallbackQuestions)
        {
            var response = new QuizResponse(request, string.Empty);

            SetRandomSkillAnswer(response, _fallbackVariants);

            var fallbackQuestionsRandom = new Random();
            int fallbackQuestionIndex = fallbackQuestionsRandom.Next(fallbackQuestions.Length);

            string text = JoinString(' ', response.Response.Text, fallbackQuestions[fallbackQuestionIndex]);
            response.Response.SetText(text);

            return response;
        }

        protected void SetRandomSkillAnswer(QuizResponse response, string[] values)
        {
            string value = GetRandomSkillAnswer(response, values);
            response.Response.SetText(JoinString(' ', response.Response.Text, value));
        }

        protected string GetRandomSkillAnswer(QuizResponse response, string[] values)
        {
            string value;
            if (values.Length > 1)
            {
                var random = new Random();
                int index;
                do
                {
                    index = random.Next(values.Length);
                } while (response.SessionState.LastRandomSkillAnswerIndex != null &&
                    index == response.SessionState.LastRandomSkillAnswerIndex);
                response.SessionState.LastRandomSkillAnswerIndex = index;
                value = values[index];
            }
            else
            {
                value = values.FirstOrDefault();
            }
            return value;
        }

        protected string JoinString(char separator, params string[] values)
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
    }
}
