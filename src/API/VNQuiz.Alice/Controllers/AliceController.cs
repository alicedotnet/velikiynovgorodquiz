using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Scenes;
using VNQuiz.Alice.Services;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AliceController : ControllerBase
    {
        private readonly IScenesProvider _scenesProvider;
        private readonly ILogger<AliceController> _logger;

        public AliceController(IScenesProvider scenesProvider, ILogger<AliceController> logger)
        {
            _scenesProvider = scenesProvider;
            _logger = logger;
        }

        [HttpPost]
        public QuizResponse Post(QuizRequest request)
        {
            Scene currentScene;
            if (request.State.Session != null)
            {
                currentScene = _scenesProvider.Get(request.State.Session.CurrentScene);
            }
            else
            {
                currentScene = _scenesProvider.Get();
            }
            var response = GetResponse(request, currentScene);
            if(response != null)
            {
                response.SessionState.ConsecutiveFallbackAnswers = 0;
            }
            else
            {
                string sessionText = JsonSerializer.Serialize(request.State.Session);
                _logger.LogInformation("Unknown request. Text: {0} Session: {1}", request.Request.Command, sessionText);

                response = currentScene.Fallback(request);
                response.SessionState.ConsecutiveFallbackAnswers++;
            }

            return response;
        }

        private QuizResponse GetResponse(QuizRequest request, Scene currentScene)
        {
            if (request.Request.Nlu.Intents != null)
            {
                if (request.Request.Nlu.Intents.IsRepeat)
                {
                    return currentScene.Repeat(request);
                }
                if (request.Request.Nlu.Intents.IsHelp)
                {
                    return currentScene.Help(request);
                }
            }

            var nextScene = currentScene.MoveToNextScene(request);
            if (nextScene != null)
            {
                return nextScene.Reply(request);
            }
            return null;
        }
    }
}
