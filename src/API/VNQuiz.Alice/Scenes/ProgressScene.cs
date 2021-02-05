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

        protected override string[] FallbackQuestions => Array.Empty<string>();

        protected override SceneType CurrentScene => SceneType.ProgressScene;

        private readonly string[] _gameNotStartedQuestions = new string[]
        {
            "Сыграем?",
            "Начнем игру?"
        };

        private readonly string[] _gameStartedQuestions = new string[]
        {
            "Продолжаем?",
            "Продолжим?"
        };

        public override QuizResponseBase Help(QuizRequest request)
        {
            return Reply(request);
        }

        public override Scene? MoveToNextScene(QuizRequest request)
        {
            if(request.Request.Nlu.Intents != null)
            {
                if(request.Request.Nlu.Intents.IsNext || request.Request.Nlu.Intents.IsConfirm)
                {
                    if (request.State.Session.NextScene <= SceneType.Welcome 
                        || request.State.Session.NextScene >= SceneType.LoseGame)
                    {
                        request.State.Session.RestorePreviousState = false;
                        return _scenesProvider.Get(SceneType.StartGame);
                    }
                    return _scenesProvider.Get(request.State.Session.NextScene);
                }
                if (request.Request.Nlu.Intents.IsReject)
                {
                    if (request.State.Session.NextScene <= SceneType.Welcome
                        || request.State.Session.NextScene >= SceneType.LoseGame)
                    {
                        return _scenesProvider.Get(SceneType.EndSession);
                    }
                    return _scenesProvider.Get(SceneType.RequestEndSession);
                }
            }
            return null;
        }

        public override QuizResponseBase Repeat(QuizRequest request)
        {
            return Reply(request);
        }

        public override QuizResponseBase Fallback(QuizRequest request)
        {
            QuizResponseBase response;
            if (request.State.Session.NextScene <= SceneType.Welcome
                || request.State.Session.NextScene >= SceneType.LoseGame)
            {
                response = Fallback(request, _gameNotStartedQuestions);
            }
            else
            {
                response = Fallback(request, _gameStartedQuestions);
            }
            return response;
        }


        public override QuizResponseBase Reply(QuizRequest request)
        {
            var lockedAchievements = _achievementsService.GetAchievements(request.State.UserOrApplication.UnlockedAchievementsIds);
            double percentage = (double)request.State.UserOrApplication.UnlockedAchievementsIds.Count / (request.State.UserOrApplication.UnlockedAchievementsIds.Count + lockedAchievements.Length);
            percentage *= 100;
            int percent = (int)Math.Ceiling(percentage);

            string progressText = $"Вы разблокировали {percent}% достижений.";
            var response = new QuizGalleryResponse(request, progressText);
            if (response.SessionState.CurrentScene != CurrentScene)
            {
                if (response.SessionState.CurrentScene != SceneType.RequestEndSession
                    && response.SessionState.CurrentScene != SceneType.RulesScene
                    && response.SessionState.CurrentScene != SceneType.RequestEndSession)
                {
                    response.SessionState.NextScene = response.SessionState.CurrentScene;
                }
                response.SessionState.CurrentScene = CurrentScene;
            }
            response.SessionState.RestorePreviousState = true;

            string questionText;
            if (response.SessionState.NextScene <= SceneType.Welcome
                || response.SessionState.NextScene >= SceneType.LoseGame)
            {
                questionText = GetRandomSkillAnswer(response.SessionState, _gameNotStartedQuestions);
            }
            else
            {
                questionText = GetRandomSkillAnswer(response.SessionState, _gameStartedQuestions);
            }

            if (lockedAchievements.Any())
            {
                var lockedAchievement = lockedAchievements.First();
                string nextAchievementText = "Следующее достижение:";
                string headerText = response.Response.Text + ' ' + nextAchievementText;
                string footerText = questionText;
                response.Response.Card = new AliceGalleryCardModel()
                {
                    Header = new AliceGalleryCardHeaderModel(headerText),
                    Items = new List<AliceGalleryCardItem>()
                    {
                        new AliceGalleryCardItem()
                        {
                            Title = lockedAchievement.Title,
                            Description = lockedAchievement.Description,
                            ImageId = lockedAchievement.ImageId
                        }
                    },
                    Footer = new AliceGalleryCardFooterModel(footerText)
                };
                string text = response.Response.Text + AliceHelper.SilenceString500
                    + nextAchievementText + AliceHelper.SilenceString500
                    + lockedAchievement.Title + AliceHelper.SilenceString500 + lockedAchievement.Description;
                response.Response.SetText(text.Replace(AliceHelper.SilenceString500, "\n"), false);
                response.Response.SetTts(text);
            }
            response.Response.AppendText('\n' + questionText, false);
            response.Response.AppendTts(AliceHelper.SilenceString1000 + questionText);

            SetFallbackButtons(request, response);
            return response;
        }
    }
}
