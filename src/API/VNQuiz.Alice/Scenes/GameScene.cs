using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Services;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Scenes
{
    public class GameScene : Scene
    {
        private readonly IQuestionsService _questionsService;

        public GameScene(IQuestionsService questionsService)
        {
            _questionsService = questionsService;
        }

        public override QuizResponse Fallback(QuizRequest request)
        {
            return new QuizResponse(request, "Я тебя не поняла. Повтори еще раз");
        }

        public override Scene MoveToNextScene(QuizRequest request)
        {
            return null;
        }

        public override QuizResponse Reply(QuizRequest request)
        {
            var question = _questionsService.GetQuestion();
            var response = new QuizResponse(request, question.Text);
            foreach (var answer in question.Answers)
            {
                response.Response.Buttons.Add(new AliceButtonModel(answer));
            }
            response.SessionState.CurrentScene = SceneType.Game;
            return response;
        }
    }
}
