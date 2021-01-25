using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Scenes;

namespace VNQuiz.Alice.Models
{
    public class QuizSessionState
    {
        public int IncorrectAnswersCount { get; set; }
        public int CurrentQuestionId { get; set; }
        public List<int> AnsweredQuestionsIds { get; set; }
        public SceneType CurrentScene { get; set; }

        public QuizSessionState()
        {
            AnsweredQuestionsIds = new List<int>();
        }
    }
}
