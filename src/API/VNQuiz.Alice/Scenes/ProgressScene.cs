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
    public class ProgressScene : Scene
    {
        private readonly IAchievementsService _achievementsService;
        private readonly IScenesProvider _scenesProvider;

        public ProgressScene(IAchievementsService achievementsService, IScenesProvider scenesProvider)
        {
            _achievementsService = achievementsService;
            _scenesProvider = scenesProvider;
        }

        protected override string[] FallbackQuestions => new string[]
        {
            "Идем назад?",
            "Возвращаемся назад?"
        };

        protected override SceneType CurrentScene => SceneType.ProgressScene;

        public override QuizResponseBase Help(QuizRequest request)
        {
            return Reply(request);
        }

        public override Scene? MoveToNextScene(QuizRequest request)
        {
            if(request.Request.Nlu.Intents != null)
            {
                if(request.Request.Nlu.Intents.IsBack || request.Request.Nlu.Intents.IsConfirm)
                {
                    return _scenesProvider.Get(request.State.Session.NextScene);
                }
                if(request.Request.Nlu.Intents.IsReject)
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
            var lockedAchievements = _achievementsService.GetAchievements(request.State.UserOrApplication.UnlockedAchievementsIds);
            double percentage = (double)request.State.UserOrApplication.UnlockedAchievementsIds.Count / (request.State.UserOrApplication.UnlockedAchievementsIds.Count + lockedAchievements.Length);
            percentage *= 100;
            int percent = (int)Math.Ceiling(percentage);

            var response = new QuizGalleryResponse(request, $"Вы разблокировали {percent}% достижений");
            if(lockedAchievements.Any())
            {
                var lockedAchievement = lockedAchievements.First();
                response.Response.Card = new AliceGalleryCardModel()
                {
                    Header = new AliceGalleryCardHeaderModel("Следующее достижение"),
                    Items = new List<AliceGalleryCardItem>()
                    {
                        new AliceGalleryCardItem()
                        {
                            Title = lockedAchievement.Title,
                            Description = lockedAchievement.Description,
                            ImageId = lockedAchievement.ImageId
                        }
                    },
                    Footer = new AliceGalleryCardFooterModel(response.Response.Text)
                };
                string text = response.Response.Text + AliceHelper.SilenceString500
                    + response.Response.Card.Header.Text + AliceHelper.SilenceString500
                    + lockedAchievement.Title + AliceHelper.SilenceString500 + lockedAchievement.Description;
                response.Response.SetText(text);
            }
            if(response.SessionState.CurrentScene != CurrentScene)
            {
                if(response.SessionState.CurrentScene != SceneType.RequestEndSession)
                {
                    response.SessionState.NextScene = response.SessionState.CurrentScene;
                }
                response.SessionState.CurrentScene = CurrentScene;
            }
            response.SessionState.RestorePreviousState = true;
            response.Response.Buttons.Add(new QuizButtonModel("назад"));
            return response;
        }
    }
}
