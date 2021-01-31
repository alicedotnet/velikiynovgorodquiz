using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using VNQuiz.Core.Interfaces;
using VNQuiz.Core.Models;

namespace VNQuiz.Core
{
    public class AchievementsHelper : IAchievementsHelper
    {
        private readonly List<Achievement> _achievements;
        private readonly Dictionary<int, Achievement> _idAchievementsMap;

        public AchievementsHelper()
        {
            _achievements = new List<Achievement>();
            _idAchievementsMap = new Dictionary<int, Achievement>();
        }

        public void Initialize(string path)
        {
            var achievements = AchievementsLoader.Load(path);
            if (achievements != null)
            {
                _achievements.AddRange(achievements);
                _achievements.Sort((x, y) => x.Id.CompareTo(y.Id));
                foreach (var question in _achievements)
                {
                    _idAchievementsMap.Add(question.Id, question);
                }
            }
        }

        public Achievement[] GetAchievements(List<int> excludeIds)
        {
            if (excludeIds == null) throw new ArgumentNullException(nameof(excludeIds));

            try
            {
                return _achievements
                    .Where(x => !excludeIds.Contains(x.Id))
                    .ToArray();
            }
            catch (Exception ex)
            {
                throw new CoreException("Failed to get a achievement", ex);
            }
        }
    }
}
