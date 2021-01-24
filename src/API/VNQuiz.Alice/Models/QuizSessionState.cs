using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNQuiz.Alice.Models
{
    public class QuizSessionState
    {
        public int IncorrectAnswersCount { get; set; }
        public int CurrentQuestionId { get; set; }
        public List<int> AnsweredQuestionsIds { get; set; }
        public QuizState QuizState { get; set; }

        public QuizSessionState()
        {
            AnsweredQuestionsIds = new List<int>();
        }
    }

    public enum QuizState
    {
        GameNotStarted,
        GameStarted
    }
}
