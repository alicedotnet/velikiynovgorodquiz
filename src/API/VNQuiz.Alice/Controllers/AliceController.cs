﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var nextScene = currentScene.MoveToNextScene(request);
            if (nextScene != null)
            {
                return nextScene.Reply(request);
            }
            //TODO::add fallback logging
            return currentScene.Fallback(request);
        }
    }
}
