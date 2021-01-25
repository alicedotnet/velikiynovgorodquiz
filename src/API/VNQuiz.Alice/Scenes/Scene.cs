using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;

namespace VNQuiz.Alice.Scenes
{
    public abstract class Scene
    {
        public abstract Scene MoveToNextScene(QuizRequest request);
        public abstract QuizResponse Reply(QuizRequest request);
        public abstract QuizResponse Fallback(QuizRequest request);
    }
}
