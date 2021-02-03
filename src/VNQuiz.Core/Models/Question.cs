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
        public string Explanation { get; }
        public bool? Shuffle { get; }

        public AdditionalFact? AdditionalInfo { get; }

        [JsonConstructor]
        public Question(string text, string correctAnswer, List<string> wrongAnswers, string explanation, bool? shuffle, AdditionalFact? additionalInfo)
        {
            if (wrongAnswers.Count < 1) throw new ArgumentException("Question must have at least one wrong answer");
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Text is required");
            if (explanation.Length > 300) throw new ArgumentException($"Explanation '{explanation.Substring(0, 20)}' has {explanation.Length} symbols which is longer then 300 symbols", nameof(explanation));
            if (correctAnswer.Length <= 3) throw new ArgumentException($"Answer '{correctAnswer}' must be longer then 3 symbols", nameof(correctAnswer));
            foreach (string? wrongAnswer in wrongAnswers)
            {
                if (wrongAnswer.Length <= 3) throw new Exception($"Wrong answer '{wrongAnswer}' must be longer then 3 symbols");
            }

            Id = ++_nextId;
            Text = text;
            CorrectAnswer = correctAnswer;
            WrongAnswers = wrongAnswers;
            Explanation = explanation;
            Shuffle = shuffle;
            AdditionalInfo = additionalInfo;
        }
    }
}
