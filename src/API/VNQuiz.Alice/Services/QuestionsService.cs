using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Core.Interfaces;
using VNQuiz.Core.Models;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Services
{
    public class QuestionsService : IQuestionsService
    {
        private readonly IQuestionsHelper _questionsHelper;

        public QuestionsService(IQuestionsHelper questionsHelper)
        {
            _questionsHelper = questionsHelper ?? throw new ArgumentNullException(nameof(questionsHelper));
        }

        public Question GetQuestion() => _questionsHelper.GetQuestion(new List<int>());

        public Question GetQuestion(int id) => _questionsHelper.GetQuestion(id);
    }
}
