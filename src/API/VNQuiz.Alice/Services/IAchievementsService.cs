using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Achievements;
using VNQuiz.Core.Models;

namespace VNQuiz.Alice.Services
{
    public interface IAchievementsService
    {
        Achievement[] GetAchievements(List<int> excludeIds);
        AchievementUnlocker GetAchievementUnlocker(Type? achievementClassType);
    }
}
