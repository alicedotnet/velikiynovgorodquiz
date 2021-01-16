using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Tests.TestsInfrastructure.Models
{
    class AliceResponseWrapper : AliceResponse
    {
        public AliceResponseWrapper() : base(new AliceRequest(), string.Empty)
        {

        }
        public AliceResponseWrapper(AliceRequestBase request, string text) : base(request, text)
        {
        }

        public AliceResponseWrapper(AliceRequestBase request, string text, string tts) : base(request, text, tts)
        {
        }

        public AliceResponseWrapper(AliceRequestBase request, string text, List<AliceButtonModel> buttons) : base(request, text, buttons)
        {
        }

        public AliceResponseWrapper(AliceRequestBase request, string text, string tts, List<AliceButtonModel> buttons) : base(request, text, tts, buttons)
        {
        }
    }
}
