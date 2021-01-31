using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VNQuiz.Alice.Models
{
    public class QuizUserState
    {
        [JsonPropertyName("unlockedAchievementsIds")]
        public List<int> UnlockedAchievementsIds { get; set; }

        public Queue<int> AnsweredQuestionsIds { get; set; }


        public QuizUserState()
        {
            UnlockedAchievementsIds = new List<int>();
            AnsweredQuestionsIds = new Queue<int>();
        }
    }
}
