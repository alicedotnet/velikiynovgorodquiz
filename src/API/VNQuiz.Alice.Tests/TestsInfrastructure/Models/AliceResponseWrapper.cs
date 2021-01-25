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
        public AliceResponseWrapper()
            : base(new AliceRequest() { State = new AliceStateModel<object, object>()}, string.Empty)
        {

        }
        public AliceResponseWrapper(AliceRequestBase<object, object> request, string text, bool keepSessionState = true) : base(request, text, keepSessionState)
        {
        }

        public AliceResponseWrapper(AliceRequestBase<object, object> request, string text, string tts, bool keepSessionState = true) : base(request, text, tts, keepSessionState)
        {
        }

        public AliceResponseWrapper(AliceRequestBase<object, object> request, string text, List<AliceButtonModel> buttons, bool keepSessionState = true) : base(request, text, buttons, keepSessionState)
        {
        }

        public AliceResponseWrapper(AliceRequestBase<object, object> request, string text, string tts, List<AliceButtonModel> buttons, bool keepSessionState = true) : base(request, text, tts, buttons, keepSessionState)
        {
        }
    }
}
