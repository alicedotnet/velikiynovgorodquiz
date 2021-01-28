using System;
using System.Text.Json.Serialization;

namespace VNQuiz.Core.Models
{
    public class AdditionalFact
    {
        public string Text { get; }
        public string? Link { get; }
        public string? PictureId { get; }

        [JsonConstructor]
        public AdditionalFact(string text, string? link, string? pictureId)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));

            Text = text;
            Link = link;
            PictureId = pictureId;
        }
    }
}
