using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Scenes;

namespace VNQuiz.Alice.Achievements
{
    public class WinGameAchievementUnlocker : AchievementUnlocker
    {
        public override bool CanUnlock(QuizRequest request)
        {
            return request.State.Session.CurrentScene == SceneType.WinGame;
        }
    }
}
