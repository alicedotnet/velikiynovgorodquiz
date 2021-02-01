using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;

namespace VNQuiz.Alice.Achievements
{
    public abstract class ConsecutiveAnswersAchievementUnlocker : AchievementUnlocker
    {
        protected abstract int ConsecutiveAnswers { get; }

        public override bool CanUnlock(QuizRequest request)
        {
            return request.State.Session.MaxConsecutiveCorrectAnswers >= ConsecutiveAnswers
                || request.State.Session.CurrentConsecutiveCorrectAnswers >= ConsecutiveAnswers;
        }
    }
}
