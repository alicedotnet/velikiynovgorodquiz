using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;

namespace VNQuiz.Alice.Achievements
{
    public class FirstGameAchievementUnlocker : AchievementUnlocker
    {
        public override bool CanUnlock(QuizRequest request)
        {
            return true;
        }
    }
}
