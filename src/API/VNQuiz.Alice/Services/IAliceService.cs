using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Services
{
    public interface IAliceService
    {
        AliceQuizResponse ProcessNewSession(AliceQuizRequest request);
    }
}
