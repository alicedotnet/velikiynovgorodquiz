using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
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
        private readonly ILogger _fallbackLogger;
        private readonly ILogger _answersLogger;

        public AliceController(IScenesProvider scenesProvider
            , ILogger<AliceController> logger
            , ILoggerFactory loggerFactory)
        {
            _scenesProvider = scenesProvider;
            _logger = logger;
            _fallbackLogger = loggerFactory.CreateLogger("Fallback");
            _answersLogger = loggerFactory.CreateLogger("Answers");
        }

        [HttpPost]
        public QuizResponseBaseReturn Post(QuizRequest request)
        {
            QuizResponseBaseReturn quizReturn;
            try
            {
                PreprocessRequest(request);
                Scene currentScene;
                if (request.State?.Session != null)
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
                    _answersLogger.LogInformation("ANSWER. Request: {0}", Serialize(request));
                    if (response.SessionState != null)
                    {
                        response.SessionState.ConsecutiveFallbackAnswers = 0;
                    }
                }
                else
                {
                    _fallbackLogger.LogInformation("FALLBACK. Request: {0}", Serialize(request));

                    response = currentScene.Fallback(request);
                    response.SessionState.ConsecutiveFallbackAnswers++;
                }
                quizReturn = response;
            }
            catch(Exception e)
            {
                QuizResponseBase response = new QuizResponse(request, "Кажется у меня что-то сломалось. Не переживайте, мой создатель скоро это увидит и все исправит!");
                quizReturn = response;
                _logger.LogError(e, Serialize(request));
            }
            return quizReturn;
        }

        private static string Serialize<T>(T value)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            };
            string result = JsonSerializer.Serialize(value, options);
            return result;
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
            if (request.Request.Command == "ping")
            {
                return new QuizResponse(request, "Привет, Яндекс! Навык работает нормально :)");
            }

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
                if(request.Request.Nlu.Intents.IsProgress)
                {
                    var progressScene = _scenesProvider.Get(SceneType.ProgressScene);
                    return progressScene.Reply(request);
                }
                if(request.Request.Nlu.Intents.IsRestart)
                {
                    var restartScene = _scenesProvider.Get(SceneType.RequestRestart);
                    return restartScene.Repeat(request);
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
