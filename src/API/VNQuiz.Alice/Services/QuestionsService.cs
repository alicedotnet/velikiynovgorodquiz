using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Models;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Services
{
    public class QuestionsService : IQuestionsService
    {
        public QuestionModel GetQuestion()
        {
            var random = new Random();
            int index = random.Next(0, _questions.Length);
            return _questions[index];
        }

        public QuestionModel GetQuestion(int id)
        {
            return _questions.FirstOrDefault(x => x.QuestionId == id);
        }

        private readonly QuestionModel[] _questions = new QuestionModel[]
        {
            new QuestionModel()
            {
                QuestionId = 1,
                CorrectAnswer = "1044",
                Text = "Отлично, начинаем! Вот первый вопрос: \nВ каком году Новгородский Кремль был впервые упомянут в летописи ?",
                Explanation = "Первое упоминание о Кремле было в 1045 году согласно \"Повести временных лет\".\nРассказать еще о Кремле?",
                WrongAnswers = new string []{"987", "1211" }
            },
            //new QuestionModel()
            //{
            //    Text = "Не совсем. Первое упоминание о Кремле было в 1045 году согласно \"Повести временных лет\".\nРассказать еще о Кремле?",
            //    Answers = new List<AliceButtonModel>()
            //    {
            //        new AliceButtonModel("да"),
            //        new AliceButtonModel("нет")
            //    }
            //},
            //new QuestionModel()
            //{
            //    Text = "Первое упоминание о крепости на берегах Волхова относят к 1045-му году, согласно «Повести временных лет». Это был период княжения Ярослава Мудрого, по приказу которого началось строительство каменного Софийского собора.\nУзнать еще больше информации и посмотреть доступные варианты экскурсий можете на сайте Новгородского Кремля. Переходим к следующему вопросу?",
            //    Answers = new List<AliceButtonModel>()
            //    {
            //        new AliceButtonModel("да"),
            //        new AliceButtonModel("сайт Кремля", false, null, new Uri("https://www.google.com/"))
            //    }
            //},
            //new QuestionModel()
            //{
            //    Text = "Ты можешь сделать еще одну ошибку. Переходим к следующему вопросу. Как известно, у скандинавов Новгород назывался Хольмгардом. Что означает это название?",
            //    Answers = new List<AliceButtonModel>()
            //    {
            //        new AliceButtonModel("Великий город"),
            //        new AliceButtonModel("Город, исчезающий под водой во время паводков"),
            //        new AliceButtonModel("Город на холме"),
            //    }
            //},
            //new QuestionModel()
            //{
            //    Text = "На самом деле Хольмгард означает город, исчезающий под водой во время паводков.  Давай дальше без ошибок. Переходим к следующему вопросу.\nВ каком году в Новгороде был запущен первый троллейбус?",
            //    Answers = new List<AliceButtonModel>()
            //    {
            //        new AliceButtonModel("1995"),
            //        new AliceButtonModel("1984"),
            //        new AliceButtonModel("1973"),
            //    }
            //},
            //new QuestionModel()
            //{
            //    Text = "Первый троллейбус был запущен в 1995 году. К сожалению, наша игра подошла к концу. Твой результат лучше 97% пользователей. Сыграем еще раз?",
            //    Answers = new List<AliceButtonModel>()
            //    {
            //        new AliceButtonModel("да"),
            //        new AliceButtonModel("нет")
            //    }
            //},
            //new QuestionModel()
            //{
            //    Text = "Спасибо за игру. До новых встреч!",
            //    Answers = new List<AliceButtonModel>()
            //},
        };
    }
}
