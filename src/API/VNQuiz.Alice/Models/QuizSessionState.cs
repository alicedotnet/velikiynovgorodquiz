using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Scenes;
using VNQuiz.Core.Models;

namespace VNQuiz.Alice.Models
{
    public class QuizSessionState
    {
        public int IncorrectAnswersCount { get; set; }
        public int CurrentQuestionId { get; set; }
        public string[] CurrentQuestionAnswers { get; set; }
        public SceneType CurrentScene { get; set; }
        public SceneType NextScene { get; set; }
        public bool RestorePreviousState { get; set; }
        public int? LastRandomSkillAnswerIndex { get; set; }
        public int ConsecutiveFallbackAnswers { get; set; }
        public List<AchievementModel> UnlockedAchievements { get; set; }

        public QuizSessionState()
        {
            CurrentQuestionAnswers = Array.Empty<string>();
            UnlockedAchievements = new List<AchievementModel>();
        }
    }
}
