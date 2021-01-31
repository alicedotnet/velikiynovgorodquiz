using System;
using System.Linq;
using System.Collections.Generic;
using VNQuiz.Core.Util;
using VNQuiz.Core.Interfaces;
using VNQuiz.Core.Models;

namespace VNQuiz.Core
{
    public class QuestionsHelper : IQuestionsHelper
    {
        private readonly List<Question> _questions;
        private readonly Dictionary<int, Question> _idQuestionMap;

        public QuestionsHelper()
        {
            _questions = new List<Question>();
            _idQuestionMap = new Dictionary<int, Question>();
        }

        public int Initialize(string path)
        {
            var questions = QuestionsLoader.Load(path);
            if (questions != null)
            {
                _questions.AddRange(questions);
                _questions.Sort((x, y) => x.Id.CompareTo(y.Id));
                foreach (var question in _questions)
                {
                    _idQuestionMap.Add(question.Id, question);
                }
                return _questions.Count;
            }
            return 0;
        }

        public Question? GetQuestion(List<int> ids)
        {
            if (ids == null) throw new ArgumentNullException(nameof(ids));
            if (_questions.Count == 0)
                return null;

            try
            {
                var remainingQuestionsIds = _idQuestionMap.Keys
                    .Where(x => !ids.Contains(x))
                    .ToArray();
                if(remainingQuestionsIds.Length > 0)
                {
                    var random = new Random();
                    var index = random.Next(remainingQuestionsIds.Length);
                    var id = remainingQuestionsIds[index];
                    return _idQuestionMap[id];
                }
                return null;
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
