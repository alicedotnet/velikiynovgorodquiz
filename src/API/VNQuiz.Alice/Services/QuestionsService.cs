using System;
using System.Collections.Generic;
using System.Linq;
using VNQuiz.Alice.Models;
using VNQuiz.Core.Interfaces;
using VNQuiz.Core.Models;

namespace VNQuiz.Alice.Services
{
    public class QuestionsService : IQuestionsService
    {
        private readonly IQuestionsHelper _questionsHelper;

        public QuestionsService(IQuestionsHelper questionsHelper)
        {
            _questionsHelper = questionsHelper ?? throw new ArgumentNullException(nameof(questionsHelper));
        }

        public List<int> GetQuestionsIds()
        {
            var random = new Random();
            var questions = _questionsHelper.GetQuestions().Select(x => x.Id).ToList();
            random.Shuffle(questions);
            return questions;
        }

        public Question? GetQuestion(QuizRequest request)
        {
            if(request.State.Session.UnansweredQuestionsIds.Any())
            {
                int i = 0;
                int questionId;
                do
                {
                    questionId = request.State.Session.UnansweredQuestionsIds[i];
                    i++;
                } while (request.State.UserOrApplication.AnsweredQuestionsIds.Contains(questionId));
                return GetQuestion(questionId);
            }
            return null;
        }

        public Question? GetQuestion(List<int> excludeIds) => _questionsHelper.GetQuestion(excludeIds);

        public Question GetQuestion(int id) => _questionsHelper.GetQuestion(id) ?? throw new Exception($"Can't find question with id {id}");
    }

    static class RandomExtensions
    {
        public static void Shuffle<T>(this Random rng, List<T> array)
        {
            int n = array.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
