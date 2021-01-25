using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Models
{
    public class QuizResponse : AliceResponse<QuizSessionState, object>
    {
        public QuizResponse(AliceRequestBase<QuizSessionState, object> request, string text) : base(request, text)
        {
        }

        public QuizResponse(AliceRequestBase<QuizSessionState, object> request, string text, string tts) : base(request, text, tts)
        {
        }

        public QuizResponse(AliceRequestBase<QuizSessionState, object> request, string text, List<AliceButtonModel> buttons) : base(request, text, buttons)
        {
        }

        public QuizResponse(AliceRequestBase<QuizSessionState, object> request, string text, string tts, List<AliceButtonModel> buttons) : base(request, text, tts, buttons)
        {
        }
    }
}
