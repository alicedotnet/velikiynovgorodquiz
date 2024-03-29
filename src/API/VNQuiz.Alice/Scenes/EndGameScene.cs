﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;
using VNQuiz.Core.Models;
using Yandex.Alice.Sdk.Helpers;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Scenes
{
    public abstract class EndGameScene : Scene
    {
        protected override string[] FallbackQuestions => new string[]
        {
            "Хотите сыграть еще?",
            "Начнем игру сначала?"
        };

        private readonly string[] _achievementTexts = new string[]
        {
            "Вы разблокировали новое достижение!",
            "Поздравляю! Вы разблокировали достижение!"
        };

        private readonly string[] _achievementsTexts = new string[]
        {
            "Вы разблокировали новые достижения!",
            "Поздравляю! Вы разблокировали достижения!"
        };

        private readonly string[] _unlockedAllTexts = new string[]
        {
            "Ура! Вы разблокировали все достижения!",
            "Поздравляю! Вы разблокировали все достижения!"
        };

        private readonly IScenesProvider _scenesProvider;
        private readonly IAchievementsService _achievementsService;

        protected abstract string[] ReplyVariations { get; }


        public EndGameScene(IScenesProvider scenesProvider, IAchievementsService achievementsService)
        {
            _scenesProvider = scenesProvider;
            _achievementsService = achievementsService;
        }

        public override Scene? MoveToNextScene(QuizRequest request)
        {
            if (request.Request.Nlu.Intents != null)
            {
                if (request.Request.Nlu.Intents.IsConfirm
                    || request.Request.Nlu.Intents.IsStart)
                {
                    return _scenesProvider.Get(SceneType.StartGame);
                }
                else if (request.Request.Nlu.Intents.IsReject)
                {
                    return _scenesProvider.Get(SceneType.EndSession);
                }
            }
            return null;
        }

        public override QuizResponseBase Reply(QuizRequest request)
        {
            request.State.Session.CurrentScene = CurrentScene;

            var response = new QuizGalleryResponse(request, string.Empty);

            var achievements = _achievementsService.GetAchievements(request.State.UserOrApplication.UnlockedAchievementsIds);
            if(response.SessionState.UnlockedAchievements.Count == 0)
            {
                foreach (var achievement in achievements)
                {
                    var achievementUnlocker = _achievementsService.GetAchievementUnlocker(achievement.AchievementUnlocker);
                    if (achievementUnlocker.CanUnlock(request))
                    {
                        response.SessionState.UnlockedAchievements.Add(new AchievementModel(achievement));
                        response.UserOrApplicationState.UnlockedAchievementsIds.Add(achievement.Id);
                    }
                }
            }

            string? replyVariationText = GetRandomSkillAnswer(response.SessionState, ReplyVariations);
            if (response.SessionState.UnlockedAchievements.Any())
            {
                var remainingAchievements = _achievementsService.GetAchievements(request.State.UserOrApplication.UnlockedAchievementsIds);
                string headerText = replyVariationText;
                string headerAchievementText;
                if(remainingAchievements.Length == 0)
                {
                    headerAchievementText = GetRandomSkillAnswer(response.SessionState, _unlockedAllTexts);
                }
                else if(response.SessionState.UnlockedAchievements.Count > 1)
                {
                    headerAchievementText = GetRandomSkillAnswer(response.SessionState, _achievementsTexts);
                }
                else
                {
                    headerAchievementText = GetRandomSkillAnswer(response.SessionState, _achievementTexts);
                }
                headerText = JoinString(' ', headerText, headerAchievementText);

                response.Response.Card = new AliceGalleryCardModel()
                {
                    Header = new AliceGalleryCardHeaderModel(headerText),
                    Items = response.SessionState.UnlockedAchievements
                        .Take(5)
                        .Select(x => new AliceGalleryCardItem()
                        {
                            Title = x.Title,
                            Description = x.Description,
                            ImageId = x.ImageId
                        }).ToList(),
                    Footer = new AliceGalleryCardFooterModel(GetRandomSkillAnswer(response.SessionState, FallbackQuestions))
                };

                string text = headerText + AliceHelper.SilenceString500;
                foreach (var item in response.Response.Card.Items)
                {
                    text += JoinString(' ', item.Title + ".", item.Description + ".") + AliceHelper.SilenceString500;
                }
                response.Response.SetText(text);
            }
            else
            {
                response.Response.SetText(JoinString(' ', response.Response.Text, replyVariationText));
            }

            string questionText = GetRandomSkillAnswer(FallbackQuestions);
            response.Response.AppendText('\n' + questionText);
            SetFallbackButtons(request, response);
            return response;
        }

        public override QuizResponseBase Repeat(QuizRequest request)
        {
            if (request.Request.Nlu.Intents != null)
            {
                if (request.Request.Nlu.Intents.IsRepeat
                    && request.Request.Nlu.Intents.Repeat != null
                    && request.Request.Nlu.Intents.Repeat.Slots != null
                    && request.Request.Nlu.Intents.Repeat.Slots.Game != null)
                {
                    var startGameScene = _scenesProvider.Get(SceneType.StartGame);
                    return startGameScene.Reply(request);
                }
            }
            return Reply(request);
        }

        public override QuizResponseBase Help(QuizRequest request)
        {
            return Reply(request);
        }

        protected override void SetFallbackButtons(QuizRequest request, QuizResponseBase response)
        {
            base.SetFallbackButtons(request, response);
            response.Response.Buttons.Add(new QuizButtonModel("прогресс"));
        }
    }
}
