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
            var fallbackVariantsRandom = new Random();
            int fallbackVariantIndex = fallbackVariantsRandom.Next(_fallbackVariants.Length);

            var fallbackQuestionsRandom = new Random();
            int fallbackQuestionIndex = fallbackQuestionsRandom.Next(FallbackQuestions.Length);

            string text = string.Join(' ', _fallbackVariants[fallbackVariantIndex], FallbackQuestions[fallbackQuestionIndex]);

            return new QuizResponse(request, text);
        }

        protected string GetRandomString(string[] values)
        {
            var random = new Random();
            int index = random.Next(values.Length);
            return values[index];
        }
    }
}
