using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Tests.TestsInfrastructure.Models
{
    class AliceResponseWrapper : QuizResponse
    {
        public AliceResponseWrapper()
            : base(new AliceRequestBase<QuizSessionState, QuizUserState>() { State = new AliceStateModel<QuizSessionState, QuizUserState>()}, string.Empty)
        {

        }
    }
}
