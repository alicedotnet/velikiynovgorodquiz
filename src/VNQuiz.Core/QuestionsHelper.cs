using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Question? GetQuestion(int id) => _questions.FirstOrDefault(q => q.Id == id);
    }
}
