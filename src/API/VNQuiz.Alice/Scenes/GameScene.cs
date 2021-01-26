﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Scenes
{
    public class GameScene : Scene
    {
        private readonly IQuestionsService _questionsService;
        private readonly IScenesProvider _scenesProvider;

        public GameScene(IQuestionsService questionsService, IScenesProvider scenesProvider)
        {
            _questionsService = questionsService;
            _scenesProvider = scenesProvider;
        }

        public override QuizResponse Fallback(QuizRequest request)
        {
            return new QuizResponse(request, "Я тебя не поняла. Повтори еще раз");
        }

        public override Scene MoveToNextScene(QuizRequest request)
        {
            int currentQuestionId = request.State.Session.CurrentQuestionId;
            var question = _questionsService.GetQuestion(currentQuestionId);
            if (request.Request.Command == question.CorrectAnswer)
            {
                return _scenesProvider.Get(SceneType.CorrectAnswer);
            }
            else if (question.WrongAnswers.Contains(request.Request.Command))
            {
                return _scenesProvider.Get(SceneType.WrongAnswer);
            }
            return null;
        }

        public override QuizResponse Reply(QuizRequest request)
        {
            var question = _questionsService.GetQuestion();
            var response = new QuizResponse(request, question.Text);
            foreach (var wrongAnswer in question.WrongAnswers)
            {
                response.Response.Buttons.Add(new AliceButtonModel(wrongAnswer));
            }
            var random = new Random();
            int correctAnswerIndex = random.Next(0, 2);
            response.Response.Buttons.Insert(correctAnswerIndex, new AliceButtonModel(question.CorrectAnswer));
            response.SessionState.CurrentScene = SceneType.Game;
            response.SessionState.CurrentQuestionId = question.QuestionId;
            return response;
        }
    }
}
