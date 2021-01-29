﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;

namespace VNQuiz.Alice.Scenes
{
    public class WrongAnswerScene : AnswerScene
    {
        protected override string[] FallbackQuestions => Array.Empty<string>();

        protected override string[] AnswerTips { get; }

        public WrongAnswerScene(IQuestionsService questionsService, IScenesProvider scenesProvider)
            : base(questionsService, scenesProvider)
        {
            AnswerTips = new string[] { "Не совсем.", "Немножко не так." };
        }

        public override QuizResponse Fallback(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override Scene MoveToNextScene(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override QuizResponse Reply(QuizRequest request)
        {
            QuizResponse response;
            request.State.Session.IncorrectAnswersCount++;
            if (request.State.Session.IncorrectAnswersCount >= 3)
            {
                var wrongAnswerResponse = base.Reply(request);
                var loseGameScene = ScenesProvider.Get(SceneType.LoseGame);
                response = loseGameScene.Reply(request);
                response.Response.SetText(JoinString('\n', wrongAnswerResponse.Response.Text, response.Response.Text));
            }
            else
            {
                response = base.Reply(request);
            }
            return response;
        }

        protected override string GetSupportText(QuizSessionState quizSessionState)
        {
            if(quizSessionState.IncorrectAnswersCount == 1)
            {
                return "Вы можете сделать еще две ошибки.";
            }
            else if(quizSessionState.IncorrectAnswersCount == 2)
            {
                return "Давай дальше без ошибок!";
            }
            return string.Empty;
        }

        public override QuizResponse Repeat(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override QuizResponse Help(QuizRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
