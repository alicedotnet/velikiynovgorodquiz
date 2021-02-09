using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using VNQuiz.Core.Models;

namespace VNQuiz.Core
{
    public static class AchievementsLoader
    {
        public static List<Achievement>? Load(string path)
        {
            return Loader.Load<Achievement>(path);
        }
    }
}
