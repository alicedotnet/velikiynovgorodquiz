using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace VNQuiz.Alice.Scenes
{
    public class ScenesProvider : IScenesProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ScenesProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Scene Get(SceneType type = SceneType.Default)
        {
            return type switch
            {
                SceneType.Default => Get<WelcomeScene>(),
                SceneType.Welcome => Get<WelcomeScene>(),
                SceneType.StartGame => Get<StartGameScene>(),
                SceneType.Question => Get<QuestionScene>(),
                SceneType.CorrectAnswer => Get<CorrectAnswerScene>(),
                SceneType.WrongAnswer => Get<WrongAnswerScene>(),
                SceneType.AdditionalInfo => Get<AdditionalInfoScene>(),
                SceneType.WinGame => Get<WinGameScene>(),
                SceneType.LoseGame => Get<LoseGameScene>(),
                SceneType.RequestEndSession => Get<RequestEndSessionScene>(),
                SceneType.RequestRestart => Get<RequestRestartScene>(),
                SceneType.ProgressScene => Get<ProgressScene>(),
                SceneType.RulesScene => Get<RulesScene>(),
                SceneType.EndSession => Get<EndSessionScene>(),
                _ => throw new Exception("Unknown scene"),
            };
        }

        private T Get<T>()
            where T : notnull
        {
            return _serviceProvider.GetRequiredService<T>()!;
        }
    }
}
