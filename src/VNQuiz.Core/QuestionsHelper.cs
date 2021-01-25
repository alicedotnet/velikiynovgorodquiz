using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNQuiz.Core
{
    public class QuestionsHelper
    {
        private const int QUESTIONS_COUNT = 10;

        private readonly List<Question> _questions;

        public QuestionsHelper()
        {
            _questions = new List<Question>();
        }

        public async Task Initialize(string path)
        {
            var loader = new QuestionsLoader();
            var questions = await loader.Load(path);
            if (questions != null)
            {
                _questions.AddRange(questions);
            }
        }

        public List<Question> GetQuestions()
        {
            if (_questions.Count < QUESTIONS_COUNT)
                return _questions;

            var indexes = new HashSet<int>();
            var random = new Random();

            while (indexes.Count != QUESTIONS_COUNT)
            {
                var index = random.Next(0, _questions.Count);
                indexes.Add(index);
            }

            return indexes.Select(i => _questions[i]).ToList();
        }
    }
}
