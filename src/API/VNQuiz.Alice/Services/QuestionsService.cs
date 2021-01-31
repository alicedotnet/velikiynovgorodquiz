using System;
using System.Collections.Generic;
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

        public Question? GetQuestion(List<int> excludeIds) => _questionsHelper.GetQuestion(excludeIds);

        public Question GetQuestion(int id) => _questionsHelper.GetQuestion(id) ?? throw new Exception($"Can't find question with id {id}");
    }
}
