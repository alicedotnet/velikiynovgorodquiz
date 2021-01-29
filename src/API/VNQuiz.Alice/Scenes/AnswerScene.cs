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
            request.State.Session.AnsweredQuestionsIds.Add(question.Id);
            QuizResponse response;
            string supportText = string.Empty;
            if (request.State.Session.IncorrectAnswersCount < 3)
            {
                response = questionScene.Reply(request);
                supportText = "\n" + GetSupportText(response.SessionState);
            }
            else
            {
                response = new QuizResponse(request, string.Empty);
            }

            string text = JoinString(' ', GetRandomSkillAnswer(response, AnswerTips), question.Explanation, supportText);
            response.Response.Text = JoinString(' ', text, response.Response.Text);
            response.Response.Tts = JoinString(' ', text, response.Response.Tts);
            return response;
        }

        protected abstract string GetSupportText(QuizSessionState quizSessionState);

    }
}
