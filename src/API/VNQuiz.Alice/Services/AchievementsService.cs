using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Achievements;
using VNQuiz.Core.Interfaces;
using VNQuiz.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace VNQuiz.Alice.Services
{
    public class AchievementsService : IAchievementsService
    {
        private readonly IAchievementsHelper _achievementsHelper;
        private readonly IServiceProvider _serviceProvider;

        public AchievementsService(IAchievementsHelper achievementsHelper, IServiceProvider serviceProvider)
        {
            _achievementsHelper = achievementsHelper;
            _serviceProvider = serviceProvider;
        }

        public Achievement[] GetAchievements(List<int> excludeIds)
        {
            return _achievementsHelper.GetAchievements(excludeIds);
        }

        public AchievementUnlocker GetAchievementUnlocker(Type? achievementClassType)
        {
            object service = _serviceProvider.GetRequiredService(achievementClassType ?? throw new Exception("Type is null"));
            if(service is AchievementUnlocker achievementBase)
            {
                return achievementBase;
            }
            throw new Exception($"Type '{achievementClassType}' is not convertible to {nameof(Achievement)}");
        }
    }
}
