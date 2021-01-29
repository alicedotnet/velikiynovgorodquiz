using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;

namespace VNQuiz.Alice.Scenes
{
    public class EndSessionScene : Scene
    {
        protected override string[] FallbackQuestions => Array.Empty<string>();
        protected override SceneType CurrentScene => SceneType.EndSession;

        public override QuizResponse Fallback(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override QuizResponse Help(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override Scene MoveToNextScene(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override QuizResponse Repeat(QuizRequest request)
        {
            throw new NotImplementedException();
        }

        public override QuizResponse Reply(QuizRequest request)
        {
            var response = new QuizResponse(
                request,
                "Очень жаль это слышать. Буду ждать еще!");
            response.Response.EndSession = true;
            return response;
        }
    }
}
