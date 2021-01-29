using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNQuiz.Alice.Scenes
{
    public class LoseGameScene : EndGameScene
    {
        protected override string[] ReplyVariations => new string[]
        {
            "К сожалению, наша игра подошла к концу."
        };

        protected override SceneType CurrentScene => SceneType.LoseGame;

        public LoseGameScene(IScenesProvider scenesProvider) : base(scenesProvider)
        {
        }
    }
}
