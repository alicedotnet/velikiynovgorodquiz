using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VNQuiz.Core.Models
{
    public class Question
    {
        private static int _nextId;

        public int Id { get; }
        public string Text { get; }
        public string CorrectAnswer { get; }
        public List<string> WrongAnswers { get; }
        public string? Explanation { get; }
        public AdditionalFact? AdditionalInfo { get; }

        [JsonConstructor]
        public Question(string text, string correctAnswer, List<string> wrongAnswers, string? explanation, AdditionalFact? additionalInfo)
        {
            if (wrongAnswers.Count < 1) throw new ArgumentException("Question must have at least one wrong answer");

            Id = ++_nextId;
            Text = text;
            CorrectAnswer = correctAnswer;
            WrongAnswers = wrongAnswers;
            Explanation = explanation;
            AdditionalInfo = additionalInfo;
        }
    }
}
