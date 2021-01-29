using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using VNQuiz.Core.Models;

namespace VNQuiz.Core
{
    static class QuestionsLoader
    {
        public static List<Question>? Load(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path)) throw new ArgumentException($"File does not exist {path}");

            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            };
            var text = File.ReadAllText(path, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<Question>>(text, options);
        }
    }
}
