using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNQuiz.Alice.Scenes
{
    public class WinGameScene : EndGameScene
    {
        protected override string[] ReplyVariations => new string[]
        {
            "Поздравляю! Вы ответили на все вопросы викторины!"
        };
        protected override SceneType CurrentScene => SceneType.WinGame;

        public WinGameScene(IScenesProvider scenesProvider) : base(scenesProvider)
        {
        }
    }
}
