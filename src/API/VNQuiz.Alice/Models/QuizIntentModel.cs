using System;
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
        public AliceIntentModel? YandexConfirm { get; set; }

        [JsonPropertyName("confirm")]
        public AliceIntentModel? Confirm { get; set; }

        public bool IsConfirm
        {
            get
            {
                return YandexConfirm != null || Confirm != null;
            }
        }

        [JsonPropertyName(AliceConstants.AliceIntents.Reject)]
        public AliceIntentModel? YandexReject { get; set; }

        [JsonPropertyName("reject")]
        public AliceIntentModel? Reject { get; set; }
    
        public bool IsReject
        {
            get
            {
                return (YandexReject != null || Reject != null) && !IsHelp;
            }
        }

        [JsonPropertyName(AliceConstants.AliceIntents.Repeat)]
        public AliceIntentModel? YandexRepeat { get; set; }

        public bool IsRepeat
        {
            get
            {
                return YandexRepeat != null;
            }
        }

        [JsonPropertyName(AliceConstants.AliceIntents.Help)]
        public AliceIntentModel? YandexHelp { get; set; }

        [JsonPropertyName("help")]
        public AliceIntentModel? Help { get; set; }

        public bool IsHelp
        {
            get
            {
                return YandexHelp != null || Help != null;
            }
        }

        [JsonPropertyName("exit")]
        public AliceIntentModel? Exit { get; set; }

        public bool IsExit
        {
            get
            {
                return Exit != null;
            }
        }

        [JsonPropertyName(AliceConstants.AliceIntents.WhatCanYouDo)]
        public AliceIntentModel? WhatCanYouDo { get; set; }

        public bool IsWhatCanYouDo
        {
            get
            {
                return WhatCanYouDo != null;
            }
        }

        [JsonPropertyName("rules")]
        public AliceIntentModel? Rules { get; set; }

        public bool IsRules
        {
            get
            {
                return Rules != null;
            }
        }

        [JsonPropertyName("answer")]
        public AliceIntentModel<AnswerSlots>? Answer { get; set; }
    }

    public class AnswerSlots
    {
        [JsonPropertyName("number")]
        public AliceEntityNumberModel? Number { get; set; }

        [JsonPropertyName("exactAnswer")]
        public AliceEntityStringModel? ExactAnswer { get; set; }
    }
}
