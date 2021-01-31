using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
        public QuizResponseBaseReturn Post(QuizRequest request)
        {
            try
            {
                PreprocessRequest(request);
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
                QuizResponseBaseReturn quizReturn = response;
                return quizReturn;
            }
            catch(Exception e)
            {
                string requestValue = JsonSerializer.Serialize(request);
                _logger.LogError(e, requestValue);
                throw;
            }
        }

        private static void PreprocessRequest(QuizRequest request)
        {
            if(request.Request.OriginalUtterance == "/reset")
            {
                if(request.State.Application != null)
                {
                    request.State.Application = new QuizUserState();
                }
                if(request.State.User != null)
                {
                    request.State.User = new QuizUserState();
                }
            }
        }

        private QuizResponseBase? GetResponse(QuizRequest request, Scene currentScene)
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
                if(request.Request.Nlu.Intents.IsWhatCanYouDo || request.Request.Nlu.Intents.IsRules)
                {
                    var rulesScene = _scenesProvider.Get(SceneType.RulesScene);
                    return rulesScene.Reply(request);
                }
                if (request.Request.Nlu.Intents.IsExit)
                {
                    var requestEndSessionScene = _scenesProvider.Get(SceneType.RequestEndSession);
                    return requestEndSessionScene.Reply(request);
                }
                var easterEggResponse = EasterEggResponse(request);
                if(easterEggResponse != null)
                {
                    return easterEggResponse;
                }
            }

            var nextScene = currentScene.MoveToNextScene(request);
            if (nextScene != null)
            {
                return nextScene.Reply(request);
            }
            return null;
        }

        private static QuizResponse? EasterEggResponse(QuizRequest request)
        {
            if((request.Request.Nlu.Tokens.Contains("жыве") || request.Request.Nlu.Tokens.Contains("живе") || request.Request.Nlu.Tokens.Contains("живи"))
                && request.Request.Nlu.Tokens.Contains("беларусь"))
            {
                return new QuizResponse(request, "Жыве вечна!");
            }
            return null;
        }
    }
}
