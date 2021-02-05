using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;
using VNQuiz.Core.Models;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Scenes
{
    public abstract class AnswerScene : Scene
    {
        protected IQuestionsService QuestionsService { get; }
        protected IScenesProvider ScenesProvider { get; }
        protected abstract string[] AnswerTips { get; }
        protected override string[] FallbackQuestions => new string[]
        {
            "Рассказать еще?",
            "Рассказать подробней?"
        };

        protected AnswerScene(IQuestionsService questionsService, IScenesProvider scenesProvider)
        {
            QuestionsService = questionsService;
            ScenesProvider = scenesProvider;
        }

        public override QuizResponseBase Reply(QuizRequest request)
        {
            string text = string.Empty;
            if (!request.State.Session.RestorePreviousState)
            {
                text = GetRandomSkillAnswer(request.State.Session, AnswerTips);
            }
            else
            {
                request.State.Session.RestorePreviousState = false;
            }


            var question = QuestionsService.GetQuestion(request.State.Session.CurrentQuestionId);
            if(request.State.UserOrApplication.AnsweredQuestionsIds.Count >= Settings.AnsweredQuestionsToKeep
                && request.State.UserOrApplication.AnsweredQuestionsIds.Count > 0)
            {
                request.State.UserOrApplication.AnsweredQuestionsIds.Dequeue();
            }
            request.State.UserOrApplication.AnsweredQuestionsIds.Enqueue(question.Id);
            request.State.Session.UnansweredQuestionsIds.Remove(question.Id);

            QuizResponseBase response;
            string supportText = string.Empty;
            if (request.State.Session.IncorrectAnswersCount < 3)
            {
                if(question.AdditionalInfo != null)
                {
                    response = GetAdditionalInfo(request);
                    response.Response.Text = "\n" + response.Response.Text;
                }
                else
                {
                    var questionScene = ScenesProvider.Get(SceneType.Question);
                    response = questionScene.Reply(request);
                    supportText = "\n" + GetSupportText(response.SessionState);
                }
            }
            else
            {
                response = new QuizResponse(request, string.Empty);
            }

            text = JoinString(' ', text, GetSentence(question.Explanation));
            text = JoinString('\n', text, supportText);
            response.Response.SetText(JoinString(' ', text, response.Response.Text), false);
            response.Response.SetTts(JoinString(' ', text, response.Response.Tts));
            return response;
        }


        public override Scene? MoveToNextScene(QuizRequest request)
        {
            if(request.Request.Nlu.Intents != null)
            {
                if(request.Request.Nlu.Intents.IsConfirm || request.Request.Nlu.Intents.IsMore)
                {
                    return ScenesProvider.Get(SceneType.AdditionalInfo);
                }
                else if(request.Request.Nlu.Intents.IsReject || request.Request.Nlu.Intents.IsNext)
                {
                    return ScenesProvider.Get(SceneType.Question);
                }
            }
            return null;
        }

        public override QuizResponseBase Repeat(QuizRequest request)
        {
            return GetAdditionalInfo(request);
        }

        public override QuizResponseBase Help(QuizRequest request)
        {
            return GetAdditionalInfo(request);
        }

        private QuizResponse GetAdditionalInfo(QuizRequest request)
        {
            var response = new QuizResponse(request, string.Empty);
            SetRandomSkillAnswer(response, FallbackQuestions);
            SetFallbackButtons(request, response);
            response.SessionState.CurrentScene = CurrentScene;
            return response;
        }

        protected abstract string GetSupportText(QuizSessionState quizSessionState);

    }
}
