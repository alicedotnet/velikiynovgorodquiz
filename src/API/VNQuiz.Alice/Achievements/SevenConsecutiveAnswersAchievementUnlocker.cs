using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNQuiz.Alice.Achievements
{
    public class SevenConsecutiveAnswersAchievementUnlocker : ConsecutiveAnswersAchievementUnlocker
    {
        protected override int ConsecutiveAnswers => 7;
    }
}
