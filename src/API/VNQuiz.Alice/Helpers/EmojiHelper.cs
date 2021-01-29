using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNQuiz.Alice.Helpers
{
    public static class EmojiHelper
    {
        public static readonly string ThumbsUp = char.ConvertFromUtf32(0x1F44D);
        public const string NumberOne = "1️⃣";
        public const string NumberTwo = "2️⃣";
        public const string NumberThree = "3️⃣";

        public static string GetNumberEmoji(int number)
        {
            return number switch
            {
                1 => NumberOne,
                2 => NumberTwo,
                3 => NumberThree,
                _ => throw new ArgumentException($"Can't find number emoji for {number}", nameof(number)),
            };
        }
    }
}
