using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;

namespace VNQuiz.Alice.Scenes
{
    public abstract class AnswerScene : Scene
    {
        protected IQuestionsService QuestionsService { get; }
        protected IScenesProvider ScenesProvider { get; }
        protected abstract string[] AnswerTips { get; }

        protected AnswerScene(IQuestionsService questionsService, IScenesProvider scenesProvider)
        {
            QuestionsService = questionsService;
            ScenesProvider = scenesProvider;
        }

        public override QuizResponse Reply(QuizRequest request)
        {
            var question = QuestionsService.GetQuestion(request.State.Session.CurrentQuestionId);
            var questionScene = ScenesProvider.Get(SceneType.Question);
            var response = questionScene.Reply(request);
            response.SessionState.AnsweredQuestionsIds.Add(question.Id);
            string text = JoinString(' ', GetAnswerTipText(), question.Explanation);
            text += "\n";
            text += JoinString(' ', GetSupportText(response.SessionState), response.Response.Text);
            response.Response.SetText(text);
            return response;
        }

        protected string GetAnswerTipText()
        {
            var random = new Random();
            int randomTipIndex = random.Next(AnswerTips.Length);
            return AnswerTips[randomTipIndex];
        }

        protected abstract string GetSupportText(QuizSessionState quizSessionState);

    }
}
