using System;
using System.Text.Json.Serialization;

namespace VNQuiz.Core.Models
{
    public class AdditionalFact
    {
        public string? Title { get; }
        public string Text { get; }
        public string? Link { get; }
        public string? LinkText { get; }
        public string? PictureId { get; }

        [JsonConstructor]
        public AdditionalFact(string title, string text, string? link, string? linkText, string? pictureId)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));

            Title = title;
            Text = text;
            Link = link;
            LinkText = linkText;
            PictureId = pictureId;
        }
    }
}
