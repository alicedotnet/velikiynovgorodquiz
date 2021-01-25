using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace VNQuiz.Core
{
    class QuestionsLoader
    {
        public async ValueTask<List<Question>?> Load(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path)) throw new ArgumentException($"File does not exist {path}");

            using var stream = new FileStream("questions.json", FileMode.Open, FileAccess.Read);
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            return await JsonSerializer.DeserializeAsync<List<Question>>(stream, options);
        }
    }
}
