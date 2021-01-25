using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNQuiz.Alice.Scenes
{
    public interface IScenesProvider
    {
        Scene Get(SceneType type = SceneType.Default);
    }
}
