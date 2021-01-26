using System.Collections.Generic;
using VNQuiz.Core.Models;

namespace VNQuiz.Core.Interfaces
{
    public interface IQuestionsHelper
    {
        Question? GetQuestion(List<int> ids);
        Question? GetQuestion(int id);
    }
}
