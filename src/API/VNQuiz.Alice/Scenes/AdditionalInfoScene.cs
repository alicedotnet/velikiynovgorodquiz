using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;
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

        public override QuizResponse Help(QuizRequest request)
        {
            return Reply(request);
        }

        public override Scene MoveToNextScene(QuizRequest request)
        {
            if (request.Request.Nlu.Intents != null)
            {
                if(request.Request.Nlu.Intents.IsConfirm)
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

        public override QuizResponse Repeat(QuizRequest request)
        {
            return Reply(request);
        }

        public override QuizResponse Reply(QuizRequest request)
        {
            var question = _questionsService.GetQuestion(request.State.Session.CurrentQuestionId);
            var additionalInfo = question.AdditionalInfo;
            var response = new QuizResponse(request, additionalInfo.Text);
            SetRandomSkillAnswer(response, FallbackQuestions);
            if(!string.IsNullOrEmpty(additionalInfo.PictureId))
            {
                response.Response.Card = new AliceImageCardModel()
                {
                    ImageId = additionalInfo.PictureId,
                    Title = additionalInfo.Title,
                    Description = response.Response.Text,
                    Button = new AliceImageCardButtonModel()
                    {
                        Text = additionalInfo.LinkText,
                        Url = new Uri(additionalInfo.Link),
                    }
                };
            }
            SetFallbackButtons(request, response);
            response.SessionState.CurrentScene = CurrentScene;
            return response;
        }

        protected override void SetFallbackButtons(QuizRequest request, QuizResponse response)
        {
            var question = _questionsService.GetQuestion(request.State.Session.CurrentQuestionId);
            var additionalInfo = question.AdditionalInfo;
            response.Response.Buttons.Add(new AliceButtonModel(additionalInfo.LinkText, false, null, new Uri(additionalInfo.Link)));
            response.Response.Buttons.Add(new QuizButtonModel("да"));
            response.Response.Buttons.Add(new QuizButtonModel("нет"));
        }
    }
}
