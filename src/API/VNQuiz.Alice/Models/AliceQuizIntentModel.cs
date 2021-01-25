﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Models
{
    public class AliceQuizIntentModel
    {
        [JsonPropertyName("YANDEX.CONFIRM")] //TODO::move this to library
        public AliceIntentModel<object> YandexConfirm { get; set; } //TODO::create non-generic version of AliceIntentModel

        [JsonPropertyName("confirm")]
        public AliceIntentModel<object> Confirm { get; set; } //TODO::create non-generic version of AliceIntentModel

        [JsonPropertyName("YANDEX.REJECT")] //TODO::move this to library
        public AliceIntentModel<object> YandexReject { get; set; } //TODO::create non-generic version of AliceIntentModel

        [JsonPropertyName("reject")]
        public AliceIntentModel<object> Reject { get; set; } //TODO::create non-generic version of AliceIntentModel
    }
}