using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;

namespace VNQuiz.Alice.Scenes
{
    public class WrongAnswerScene : AnswerScene
    {
        protected override SceneType CurrentScene => SceneType.WrongAnswer;

        protected override string[] AnswerTips { get; }

        public WrongAnswerScene(IQuestionsService questionsService, IScenesProvider scenesProvider)
            : base(questionsService, scenesProvider)
        {
            AnswerTips = new string[] { "Не совсем.", "Немножко не так." };
        }

        public override QuizResponseBase Reply(QuizRequest request)
        {
            QuizResponseBase response;
            request.State.Session.IncorrectAnswersCount++;
            if (request.State.Session.IncorrectAnswersCount >= 3)
            {
                var loseGameScene = ScenesProvider.Get(SceneType.LoseGame);
                response = loseGameScene.Reply(request);
                //var wrongAnswerResponse = base.Reply(request);
                //response.Response.SetText(JoinString('\n', wrongAnswerResponse.Response.Text, response.Response.Text));
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
    }
}
