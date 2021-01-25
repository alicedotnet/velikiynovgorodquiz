using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VNQuiz.Core
{
    public class Question
    {
        public string Text { get; }
        public string Answer { get; }
        public List<string> WrongAnswers { get; }

        [JsonConstructor]
        public Question(string text, string answer, List<string> wrongAnswers)
        {
            if (wrongAnswers.Count < 1) throw new ArgumentException("Question must have at least one wrong answer");

            Text = text;
            Answer = answer;
            WrongAnswers = wrongAnswers;
        }
    }
}
