using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VNQuiz.Core.Models;

namespace VNQuiz.Alice.Models
{
    public class AchievementModel
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("imageId")]
        public string? ImageId { get; set; }

        public AchievementModel()
        {

        }

        public AchievementModel(Achievement achievement)
        {
            Title = achievement.Title;
            Description = achievement.Description;
            ImageId = achievement.ImageId;
        }
    }
}
