using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;

namespace VNQuiz.Alice.Services
{
    public interface IQuestionsService
    {
        QuestionModel GetQuestion();
    }
}
