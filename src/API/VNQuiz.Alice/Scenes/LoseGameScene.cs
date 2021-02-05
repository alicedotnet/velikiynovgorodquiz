using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Services;

namespace VNQuiz.Alice.Scenes
{
    public class LoseGameScene : EndGameScene
    {
        protected override string[] ReplyVariations => new string[]
        {
            "К сожалению, наша игра подошла к концу.",
            "Очень жаль, но наша игра подошла к концу."
        };

        protected override SceneType CurrentScene => SceneType.LoseGame;

        public LoseGameScene(IScenesProvider scenesProvider, IAchievementsService achievementsService)
            : base(scenesProvider, achievementsService)
        {
        }
    }
}
