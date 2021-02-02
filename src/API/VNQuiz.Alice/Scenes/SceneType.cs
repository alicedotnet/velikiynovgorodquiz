using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNQuiz.Alice.Scenes
{
    public enum SceneType
    {
        Default,
        Welcome,
        StartGame,
        Question,
        CorrectAnswer,
        WrongAnswer,
        AdditionalInfo,
        RequestEndSession,
        RequestRestart,
        ProgressScene,
        RulesScene,
        LoseGame,
        WinGame,
        EndSession
    }
}
