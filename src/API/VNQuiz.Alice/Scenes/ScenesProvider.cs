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
                SceneType.Default => _serviceProvider.GetService<WelcomeScene>(),
                SceneType.Welcome => _serviceProvider.GetService<WelcomeScene>(),
                SceneType.Game => _serviceProvider.GetService<GameScene>(),
                SceneType.EndSession => _serviceProvider.GetService<EndSessionScene>(),
                _ => throw new Exception("Unknown scene"),
            };
        }
    }
}
