using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Scenes;

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
            try
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
                if (response != null)
                {
                    response.SessionState.ConsecutiveFallbackAnswers = 0;
                }
                else
                {
                    string requestValue = JsonSerializer.Serialize(request);
                    _logger.LogInformation("FALLBACK. Request: {0}", requestValue);

                    response = currentScene.Fallback(request);
                    response.SessionState.ConsecutiveFallbackAnswers++;
                }
                return response;
            }
            catch(Exception e)
            {
                string requestValue = JsonSerializer.Serialize(request);
                _logger.LogError(e, requestValue);
                throw;
            }
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
                if (request.Request.Nlu.Intents.IsExit)
                {
                    var requestEndSessionScene = _scenesProvider.Get(SceneType.RequestEndSession);
                    return requestEndSessionScene.Reply(request);
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
