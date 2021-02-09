using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNQuiz.Core.Models;

namespace VNQuiz.Core.Interfaces
{
    public interface IAchievementsHelper
    {
        void Initialize(string path);
        Achievement[] GetAchievements(List<int> excludeIds);
    }
}
