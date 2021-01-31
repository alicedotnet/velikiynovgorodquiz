using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;
using VNQuiz.Core.Models;

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

        public override QuizResponse Reply(QuizRequest request)
        {
            var question = QuestionsService.GetQuestion(request.State.Session.CurrentQuestionId);
            request.State.Session.AnsweredQuestionsIds.Add(question.Id);

            QuizResponse response;
            string supportText = string.Empty;
            if (request.State.Session.IncorrectAnswersCount < 3)
            {
                if(question.AdditionalInfo != null)
                {
                    response = GetAdditionalInfo(request);
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

            string text = string.Empty;
            if(!request.State.Session.RestorePreviousState)
            {
                text = GetRandomSkillAnswer(response.SessionState, AnswerTips);
            }
            else
            {
                request.State.Session.RestorePreviousState = false;
            }
            text = JoinString(' ', text, GetSentence(question.Explanation), supportText);
            response.Response.Text = JoinString(' ', text, response.Response.Text);
            response.Response.Tts = JoinString(' ', text, response.Response.Tts);
            return response;
        }


        public override Scene MoveToNextScene(QuizRequest request)
        {
            if(request.Request.Nlu.Intents != null)
            {
                if(request.Request.Nlu.Intents.IsConfirm)
                {
                    return ScenesProvider.Get(SceneType.AdditionalInfo);
                }
                else if(request.Request.Nlu.Intents.IsReject)
                {
                    return ScenesProvider.Get(SceneType.Question);
                }
            }
            return null;
        }

        public override QuizResponse Repeat(QuizRequest request)
        {
            return GetAdditionalInfo(request);
        }

        public override QuizResponse Help(QuizRequest request)
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

        protected override void SetFallbackButtons(QuizRequest request, QuizResponse response)
        {
            response.Response.Buttons.Add(new QuizButtonModel("да"));
            response.Response.Buttons.Add(new QuizButtonModel("нет"));
        }

        protected abstract string GetSupportText(QuizSessionState quizSessionState);

    }
}
