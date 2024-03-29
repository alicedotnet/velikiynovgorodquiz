﻿using System;
using System.Text.Json.Serialization;

namespace VNQuiz.Core.Models
{
    public class AdditionalFact
    {
        public string Title { get; }
        public string Text { get; }
        public Uri? Link { get; }
        public string LinkText { get; }
        public string? PictureId { get; }

        [JsonConstructor]
        public AdditionalFact(string title, string text, Uri? link, string? linkText, string? pictureId)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));

            Title = title ?? string.Empty;
            Text = text;
            Link = link;
            LinkText = linkText ?? string.Empty;
            PictureId = pictureId;
        }
    }
}
