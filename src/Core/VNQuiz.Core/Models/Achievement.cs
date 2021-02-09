using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VNQuiz.Core.Converters;

namespace VNQuiz.Core.Models
{
    public class Achievement
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("imageId")]
        public string? ImageId { get; set; }

        [JsonPropertyName("achievementUnlocker")]
        [JsonConverter(typeof(TypeConverter))]
        public Type? AchievementUnlocker { get; set; }
        [JsonPropertyName("dependsOnAchievementId")]
        public int DependsOnAchievementId { get; set; }
    }
}
