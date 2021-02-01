using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNQuiz.Alice.Achievements
{
    public class ThreeConsecutiveAnswersAchievementUnlocker : ConsecutiveAnswersAchievementUnlocker
    {
        protected override int ConsecutiveAnswers => 3;
    }
}
