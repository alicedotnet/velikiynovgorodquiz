using System;
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

        public abstract Scene MoveToNextScene(QuizRequest request);
        public abstract QuizResponse Reply(QuizRequest request);

        public abstract QuizResponse Repeat(QuizRequest request);

        public abstract QuizResponse Help(QuizRequest request);

        public virtual QuizResponse Fallback(QuizRequest request)
        {
            var response = new QuizResponse(request, string.Empty);

            SetRandomSkillAnswer(response, _fallbackVariants);

            var fallbackQuestionsRandom = new Random();
            int fallbackQuestionIndex = fallbackQuestionsRandom.Next(FallbackQuestions.Length);

            response.Response.Text = string.Join(' ', response.Response.Text, FallbackQuestions[fallbackQuestionIndex]);

            return response;
        }

        protected void SetRandomSkillAnswer(QuizResponse response, string[] values)
        {
            var random = new Random();
            int index;
            do
            {
                index = random.Next(values.Length);
            } while (response.SessionState.LastRandomSkillAnswerIndex != null &&
                index == response.SessionState.LastRandomSkillAnswerIndex);
            response.Response.Text = string.Join(' ', response.Response.Text, values[index]);
            response.SessionState.LastRandomSkillAnswerIndex = index;
        }
    }
}
