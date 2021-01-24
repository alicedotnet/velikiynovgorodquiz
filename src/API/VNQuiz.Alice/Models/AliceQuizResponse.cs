using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Models
{
    public class AliceQuizResponse : AliceResponse<QuizSessionState, object>
    {
        public AliceQuizResponse(AliceRequestBase request, string text) : base(request, text)
        {
        }

        public AliceQuizResponse(AliceRequestBase request, string text, string tts) : base(request, text, tts)
        {
        }

        public AliceQuizResponse(AliceRequestBase request, string text, List<AliceButtonModel> buttons) : base(request, text, buttons)
        {
        }

        public AliceQuizResponse(AliceRequestBase request, string text, string tts, List<AliceButtonModel> buttons) : base(request, text, tts, buttons)
        {
        }
    }
}
