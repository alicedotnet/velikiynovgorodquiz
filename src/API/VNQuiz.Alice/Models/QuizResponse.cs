using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Models
{
    public class QuizResponse : AliceImageResponse<QuizSessionState, object>
    {
        public QuizResponse(AliceRequestBase<QuizSessionState, object> request, string text) : base(request, text)
        {
        }

        public QuizResponse(AliceRequestBase<QuizSessionState, object> request, string text, string tts) : base(request, text, tts)
        {
        }

        public QuizResponse(AliceRequestBase<QuizSessionState, object> request, string text, List<QuizButtonModel> buttons) : base(request, text, buttons.Cast<AliceButtonModel>().ToList())
        {
        }

        public QuizResponse(AliceRequestBase<QuizSessionState, object> request, string text, string tts, List<QuizButtonModel> buttons) : base(request, text, tts, buttons.Cast<AliceButtonModel>().ToList())
        {
        }
    }
}
