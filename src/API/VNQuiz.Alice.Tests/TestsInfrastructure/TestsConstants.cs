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
            public const string AdditionalInfo = _assetsFolder + "AdditionalInfo.json";
            public const string Rules_GameStarted_Continue = _assetsFolder + "Rules_GameStarted_Continue.json";
            public const string Rules_NewSession_StartGame = _assetsFolder + "Rules_NewSession_StartGame.json";
        }
    }
}
