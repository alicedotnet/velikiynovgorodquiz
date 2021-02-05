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

        private readonly string[] _twoErrorsLeftTexts = new string[]
        {
            "Вы можете сделать еще две ошибки.",
            "Вы можете ошибиться еще два раза."
        };

        private readonly string[] _oneErrorLeftTexts = new string[]
        {
            "Давай дальше без ошибок!",
            "Повнимательнее! Больше ошибаться нельзя."
        };

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

                var wrongAnswerResponse = base.Reply(request);
                response.Response.Text = wrongAnswerResponse.Response.Text + '\n' + response.Response.Text;
                response.Response.SetTts(JoinString(' ', wrongAnswerResponse.Response.Tts, response.Response.Tts));
            }
            else
            {
                response = base.Reply(request);
            }
            if(response.SessionState.CurrentConsecutiveCorrectAnswers > response.SessionState.MaxConsecutiveCorrectAnswers)
            {
                response.SessionState.MaxConsecutiveCorrectAnswers = response.SessionState.CurrentConsecutiveCorrectAnswers;
            }
            response.SessionState.CurrentConsecutiveCorrectAnswers = 0;
            return response;
        }

        protected override string GetSupportText(QuizSessionState quizSessionState)
        {
            if(quizSessionState.IncorrectAnswersCount == 1)
            {
                return GetRandomSkillAnswer(_twoErrorsLeftTexts);
            }
            else if(quizSessionState.IncorrectAnswersCount == 2)
            {
                return GetRandomSkillAnswer(_oneErrorLeftTexts);
            }
            return string.Empty;
        }
    }
}
