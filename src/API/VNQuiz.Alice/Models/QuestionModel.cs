using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNQuiz.Alice.Models
{
    public class QuestionModel
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
        public string[] WrongAnswers { get; set; }
    }
}
