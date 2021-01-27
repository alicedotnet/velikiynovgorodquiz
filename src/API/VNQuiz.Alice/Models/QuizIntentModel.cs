﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Yandex.Alice.Sdk;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Models
{
    public class QuizIntentModel
    {
        [JsonPropertyName(AliceConstants.AliceIntents.Confirm)]
        public AliceIntentModel YandexConfirm { get; set; }

        [JsonPropertyName("confirm")]
        public AliceIntentModel Confirm { get; set; }

        public bool IsConfirm
        {
            get
            {
                return YandexConfirm != null || Confirm != null;
            }
        }

        [JsonPropertyName(AliceConstants.AliceIntents.Reject)]
        public AliceIntentModel YandexReject { get; set; }

        [JsonPropertyName("reject")]
        public AliceIntentModel Reject { get; set; }
    
        public bool IsReject
        {
            get
            {
                return (YandexReject != null || Reject != null) && !IsHelp;
            }
        }

        [JsonPropertyName(AliceConstants.AliceIntents.Repeat)]
        public AliceIntentModel YandexRepeat { get; set; }

        public bool IsRepeat
        {
            get
            {
                return YandexRepeat != null;
            }
        }

        [JsonPropertyName(AliceConstants.AliceIntents.Help)]
        public AliceIntentModel YandexHelp { get; set; }

        [JsonPropertyName("help")]
        public AliceIntentModel Help { get; set; }

        public bool IsHelp
        {
            get
            {
                return YandexHelp != null || Help != null;
            }
        }
    }
}
