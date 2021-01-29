using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNQuiz.Alice.Tests.TestsInfrastructure
{
    static class TestsConstants
    {
        public static class Assets
        {
            private const string _assetsFolder = "TestsInfrastructure/Assets/";
            public const string AlicePingFilePath = _assetsFolder + "AlicePing.json";
            public const string StartGameFilePath = _assetsFolder + "StartGame.json";
            public const string WrongAnswerFilePath = _assetsFolder + "WrongAnswer.json";
            public const string LastWrongAnswerFilePath = _assetsFolder + "LastWrongAnswer.json";
            public const string EndGame = _assetsFolder + "EndGame.json";
            public const string Question = _assetsFolder + "Question.json";
            public const string CorrectAnswer = _assetsFolder + "CorrectAnswer.json";
        }
    }
}
