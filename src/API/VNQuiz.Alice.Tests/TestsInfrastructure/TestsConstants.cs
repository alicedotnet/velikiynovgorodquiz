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
            private const string AssetsFolder = "TestsInfrastructure/Assets/";
            public const string AlicePingFilePath = AssetsFolder + "AlicePing.json";
            public const string StartGameFilePath = AssetsFolder + "StartGame.json";
            public const string WrongAnswerFilePath = AssetsFolder + "WrongAnswer.json";
        }
    }
}
