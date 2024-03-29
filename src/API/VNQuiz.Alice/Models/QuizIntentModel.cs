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
        public AliceIntentModel? YandexConfirm { get; set; }

        [JsonPropertyName("confirm")]
        public AliceIntentModel? Confirm { get; set; }

        [JsonIgnore]
        public bool IsConfirm
        {
            get
            {
                return (YandexConfirm != null || Confirm != null) && !IsReject;
            }
        }

        [JsonPropertyName(AliceConstants.AliceIntents.Reject)]
        public AliceIntentModel? YandexReject { get; set; }

        [JsonPropertyName("reject")]
        public AliceIntentModel? Reject { get; set; }

        [JsonIgnore]
        public bool IsReject
        {
            get
            {
                return (YandexReject != null || Reject != null) && !IsHelp && !IsNext;
            }
        }

        [JsonPropertyName(AliceConstants.AliceIntents.Repeat)]
        public AliceIntentModel? YandexRepeat { get; set; }

        [JsonPropertyName("repeat")]
        public AliceIntentModel<RepeatSlots>? Repeat { get; set; }

        [JsonIgnore]
        public bool IsRepeat
        {
            get
            {
                return YandexRepeat != null || Repeat != null;
            }
        }

        [JsonPropertyName(AliceConstants.AliceIntents.Help)]
        public AliceIntentModel? YandexHelp { get; set; }

        [JsonPropertyName("help")]
        public AliceIntentModel? Help { get; set; }

        [JsonIgnore]
        public bool IsHelp
        {
            get
            {
                return YandexHelp != null || Help != null;
            }
        }

        [JsonPropertyName("more")]
        public AliceIntentModel? More { get; set; }

        [JsonIgnore]
        public bool IsMore
        {
            get
            {
                return More != null;
            }
        }

        [JsonPropertyName("exit")]
        public AliceIntentModel? Exit { get; set; }

        [JsonIgnore]
        public bool IsExit
        {
            get
            {
                return Exit != null;
            }
        }

        [JsonPropertyName(AliceConstants.AliceIntents.WhatCanYouDo)]
        public AliceIntentModel? WhatCanYouDo { get; set; }

        [JsonIgnore]
        public bool IsWhatCanYouDo
        {
            get
            {
                return WhatCanYouDo != null && YandexConfirm == null;
            }
        }

        [JsonPropertyName("rules")]
        public AliceIntentModel? Rules { get; set; }

        [JsonIgnore]
        public bool IsRules
        {
            get
            {
                return Rules != null;
            }
        }

        [JsonPropertyName("progress")]
        public AliceIntentModel? Progress { get; set; }

        [JsonIgnore]
        public bool IsProgress
        {
            get
            {
                return Progress != null;
            }
        }

        [JsonPropertyName("next")]
        public AliceIntentModel? Next { get; set; }

        [JsonIgnore]
        public bool IsNext
        {
            get
            {
                return Next != null;
            }
        }

        [JsonPropertyName("skip")]
        public AliceIntentModel? Skip { get; set; }

        [JsonIgnore]
        public bool IsSkip
        {
            get
            {
                return Skip != null;
            }
        }

        [JsonPropertyName("start")]
        public AliceIntentModel? Start { get; set; }

        [JsonIgnore]
        public bool IsStart
        {
            get
            {
                return Start != null;
            }
        }

        [JsonPropertyName("restart")]
        public AliceIntentModel? Restart { get; set; }

        [JsonIgnore]
        public bool IsRestart
        {
            get
            {
                return Restart != null;
            }
        }

        [JsonPropertyName("answer")]
        public AliceIntentModel<AnswerSlots>? Answer { get; set; }
    }

    public class AnswerSlots
    {
        [JsonPropertyName("number")]
        public AliceEntityNumberModel? Number { get; set; }

        [JsonPropertyName("exactNumber")]
        public AliceEntityNumberModel? ExactNumber { get; set; }

        [JsonPropertyName("exactAnswer")]
        public AliceEntityStringModel? ExactAnswer { get; set; }
    }

    public class RepeatSlots
    {
        [JsonPropertyName("game")]
        public AliceEntityStringModel? Game { get; set; }
    }
}
