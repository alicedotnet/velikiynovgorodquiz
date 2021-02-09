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
            return Loader.Load<Question>(path);
        }
    }
}
