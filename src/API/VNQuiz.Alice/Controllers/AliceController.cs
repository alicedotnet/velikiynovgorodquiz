using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AliceController : ControllerBase
    {
        private readonly IAliceService _aliceService;
        private readonly ILogger<AliceController> _logger;

        public AliceController(IAliceService aliceService, ILogger<AliceController> logger)
        {
            _aliceService = aliceService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post(AliceQuizRequest request)
        {
            return Ok(ProcessRequest(request));
        }

        private AliceQuizResponse ProcessRequest(AliceQuizRequest request)
        {
            if(request.State.Session != null)
            {
                switch(request.State.Session.QuizState)
                {
                    case QuizState.GameNotStarted:
                        return _aliceService.ProcessNewSession(request);
                }
            }
            else if(request.Session.New)
            {
                return _aliceService.ProcessNewSession(request);
            }
            return new AliceQuizResponse(request, "Ой, кажется что-то пошло не так. Попробуйте еще раз");
        }

    }
}
