using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;
using Yandex.Alice.Sdk.Helpers;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Scenes
{
    public class AdditionalInfoScene : Scene
    {
        protected override string[] FallbackQuestions => new string[]
        {
            "Идем дальше?",
            "К следующему вопросу?"
        };

        protected override SceneType CurrentScene => SceneType.AdditionalInfo;

        private readonly IQuestionsService _questionsService;
        private readonly IScenesProvider _scenesProvider;

        public AdditionalInfoScene(IQuestionsService questionsService, IScenesProvider scenesProvider)
        {
            _questionsService = questionsService;
            _scenesProvider = scenesProvider;
        }

        public override QuizResponseBase Help(QuizRequest request)
        {
            return Reply(request);
        }

        public override Scene? MoveToNextScene(QuizRequest request)
        {
            if (request.Request.Nlu.Intents != null)
            {
                if(request.Request.Nlu.Intents.IsConfirm || request.Request.Nlu.Intents.IsNext)
                {
                    return _scenesProvider.Get(SceneType.Question);
                }
                else if(request.Request.Nlu.Intents.IsReject)
                {
                    return _scenesProvider.Get(SceneType.RequestEndSession);
                }
            }
            return null;
        }

        public override QuizResponseBase Repeat(QuizRequest request)
        {
            return Reply(request);
        }

        public override QuizResponseBase Reply(QuizRequest request)
        {
            var question = _questionsService.GetQuestion(request.State.Session.CurrentQuestionId);
            var additionalInfo = question.AdditionalInfo;
            QuizResponse response;
            if(additionalInfo != null)
            {
                response = new QuizResponse(request, GetSentence(additionalInfo.Text));
                SetRandomSkillAnswer(response, FallbackQuestions);
                if (!string.IsNullOrEmpty(additionalInfo.PictureId))
                {
                    response.Response.Card = new AliceImageCardModel()
                    {
                        ImageId = additionalInfo.PictureId,
                        Title = additionalInfo.Title,
                        Description = response.Response.Text,
                        Button = new AliceImageCardButtonModel()
                        {
                            Text = additionalInfo.LinkText,
                            Url = additionalInfo.Link,
                        }
                    };
                    response.Response.SetTts(additionalInfo.Title + AliceHelper.SilenceString1000 + response.Response.Text);
                    response.Response.SetText(JoinString(' ', GetSentence(additionalInfo.Title), response.Response.Text), false);
                }
                response.Response.Buttons.Add(new AliceButtonModel(additionalInfo.LinkText, false, null, additionalInfo.Link));
                response.SessionState.IsOpenedAdditionalInfo = true;
            }
            else
            {
                response = new QuizResponse(request, "По этому вопросу нет дополнительной информации.");
            }
            SetFallbackButtons(request, response);
            response.SessionState.CurrentScene = CurrentScene;
            return response;
        }
    }
}
