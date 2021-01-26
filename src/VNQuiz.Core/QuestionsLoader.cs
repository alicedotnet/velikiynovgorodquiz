using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using VNQuiz.Core.Models;

namespace VNQuiz.Core
{
    class QuestionsLoader
    {
        public List<Question>? Load(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path)) throw new ArgumentException($"File does not exist {path}");

            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            return JsonSerializer.Deserialize<List<Question>>(File.ReadAllText(path), options);
        }
    }
}
