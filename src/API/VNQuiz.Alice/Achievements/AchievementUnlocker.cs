using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;

namespace VNQuiz.Alice.Achievements
{
    public abstract class AchievementUnlocker
    {
        public abstract bool CanUnlock(QuizRequest request);
    }
}
