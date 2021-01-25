using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Models
{
    public class AliceQuizRequest : AliceRequest<AliceQuizIntentModel, QuizSessionState, object>
    {
    }
}
