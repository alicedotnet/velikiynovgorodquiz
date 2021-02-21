using System.Collections.Generic;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Scenes;
using VNQuiz.Core.Models;
using Xunit;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Tests.Scenes
{
    public class QuestionSceneTests
    {
        [Theory]
        [InlineData("возле какого озера", "возле ильмени", "Ильмень")]
        [InlineData("На какой реке расположен", "на реке волхов", "Волхов")]
        [InlineData("Как звали новгородского князя", "князь звали рюрик", "Рюрик")]
        [InlineData("Почему у Новгорода", "находок", "находки")]
        [InlineData("На каком пути", "на шелковом пути", "Шелковый путь")]
        [InlineData("На каком пути лежал", "на балтийском", "Балтийско-Волжский")]
        [InlineData("любой вопрос", "законсервированные находки", "Законсервированные находки в торфянистой почве")]
        public void FuzzyComparison_CorrectAnswer(string questionText, string userAnswer, string correctAnswer, string wrongAnswer = "синева")
        {
            var questionScene = new QuestionScene(null, null);
            var question = new Question(1, questionText, correctAnswer, new List<string>() { wrongAnswer }, string.Empty, false, null);
            var result = questionScene.IsCorrectAnswer(userAnswer, question);
            Assert.NotNull(result);
            Assert.True(result);
        }
    }
}
