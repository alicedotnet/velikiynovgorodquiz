using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Core.Models;

namespace VNQuiz.Alice.Services
{
    public interface IQuestionsService
    {
        Question? GetQuestion(List<int> excludeIds);
        Question GetQuestion(int id);
    }
}
