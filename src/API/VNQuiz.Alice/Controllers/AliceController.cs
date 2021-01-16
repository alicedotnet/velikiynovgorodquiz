using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AliceController : ControllerBase
    {
        private readonly ILogger<AliceController> _logger;

        public AliceController(ILogger<AliceController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post(AliceRequest<object, object, TempState> request)
        {
            int requestNumber = request.State.User != null ? request.State.User.RequestNumber : 0;
            if(requestNumber >= _responses.Length)
            {
                requestNumber = 0;
            }
            var tempResponse = _responses[requestNumber];
            var state = new TempState
            {
                RequestNumber = requestNumber + 1
            };
            var response = new AliceResponse(request, tempResponse.Text, tempResponse.Buttons)
            {
                UserStateUpdate = state
            };
            return Ok(response);
        }

        private readonly TempResponse[] _responses = new TempResponse[]
        {
            new TempResponse()
            {
                Text = "Привет! Предлагаю сыграть в викторину по истории Великого Новгорода. Я буду задавать тебе вопросы, а ты выбирать один из трех вариантов ответа. Игра закончится после трех неправильных ответов. Начнем?",
                Buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel("да")
                }
            },
            new TempResponse()
            {
                Text = "Отлично, начинаем! Вот первый вопрос: \nВ каком году Новгородский Кремль был впервые упомянут в летописи ?",
                Buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel("987"),
                    new AliceButtonModel("1045"),
                    new AliceButtonModel("1211"),
                }
            },
            new TempResponse()
            {
                Text = "Не совсем. Первое упоминание о Кремле было в 1045 году согласно \"Повести временных лет\".\nРассказать еще о Кремле?",
                Buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel("да"),
                    new AliceButtonModel("нет")
                }
            },
            new TempResponse()
            {
                Text = "Первое упоминание о крепости на берегах Волхова относят к 1045-му году, согласно «Повести временных лет». Это был период княжения Ярослава Мудрого, по приказу которого началось строительство каменного Софийского собора.\nУзнать еще больше информации и посмотреть доступные варианты экскурсий можете на сайте Новгородского Кремля. Переходим к следующему вопросу?",
                Buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel("да"),
                    new AliceButtonModel("сайт Кремля", false, null, new Uri("https://www.google.com/"))
                }
            },
            new TempResponse()
            {
                Text = "Ты можешь сделать еще одну ошибку. Переходим к следующему вопросу. Как известно, у скандинавов Новгород назывался Хольмгардом. Что означает это название?",
                Buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel("Великий город"),
                    new AliceButtonModel("Город, исчезающий под водой во время паводков"),
                    new AliceButtonModel("Город на холме"),
                }
            },
            new TempResponse()
            {
                Text = "На самом деле Хольмгард означает город, исчезающий под водой во время паводков.  Давай дальше без ошибок. Переходим к следующему вопросу.\nВ каком году в Новгороде был запущен первый троллейбус?",
                Buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel("1995"),
                    new AliceButtonModel("1984"),
                    new AliceButtonModel("1973"),
                }
            },
            new TempResponse()
            {
                Text = "Первый троллейбус был запущен в 1995 году. К сожалению, наша игра подошла к концу. Твой результат лучше 97% пользователей. Сыграем еще раз?",
                Buttons = new List<AliceButtonModel>()
                {
                    new AliceButtonModel("да"),
                    new AliceButtonModel("нет")
                }
            },
            new TempResponse()
            {
                Text = "Спасибо за игру. До новых встреч!",
                Buttons = new List<AliceButtonModel>()
            },
        };

        public class TempState
        {
            public int RequestNumber { get; set; }
        }

        class TempResponse
        {
            public string Text { get; set; }
            public List<AliceButtonModel> Buttons { get; set; }
        }
    }
}
