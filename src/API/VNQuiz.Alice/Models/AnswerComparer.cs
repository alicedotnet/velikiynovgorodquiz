using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace VNQuiz.Alice.Models
{
    public class AnswerComparer : IEqualityComparer<string>
    {
        public bool Equals(string? x, string? y)
        {
            return Preprocess(x) == Preprocess(y);
        }

        public int GetHashCode([DisallowNull] string obj)
        {
            return Preprocess(obj)!.GetHashCode();
        }

        private static string? Preprocess(string? value)
        {
            return AnswersModel.PrepareText(value)?.ToLower();
        }
    }
}
