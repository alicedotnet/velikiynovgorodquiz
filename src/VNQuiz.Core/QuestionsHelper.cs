using System;
using System.Collections.Generic;
using VNQuiz.Core.Util;
using VNQuiz.Core.Interfaces;
using VNQuiz.Core.Models;

namespace VNQuiz.Core
{
    public class QuestionsHelper : IQuestionsHelper
    {
        private readonly List<Question> _questions;

        public QuestionsHelper()
        {
            _questions = new List<Question>();
        }

        public void Initialize(string path)
        {
            var loader = new QuestionsLoader();
            var questions = loader.Load(path);
            if (questions != null)
            {
                _questions.AddRange(questions);
                _questions.Sort((x, y) => x.Id.CompareTo(y.Id));
            }
        }

        public Question? GetQuestion(List<int> ids)
        {
            if (ids == null) throw new ArgumentNullException(nameof(ids));
            if (_questions.Count == 0)
                return null;

            try
            {
                var random = new Random();
                var index = random.Next(_questions.Count);
                if (ids.Contains(index))
                {
                    index = random.Next(_questions.Count);
                }
                return _questions[index];
            }
            catch (Exception ex)
            {
                throw new CoreException("Failed to get a question", ex);
            }
        }

        public Question? GetQuestion(int id)
        {
            var index = _questions.BinarySearch(id, (x) => x.Id);
            return index >= 0 ? _questions[index] : null;
        }
    }
}
