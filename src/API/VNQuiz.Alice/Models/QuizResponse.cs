using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace VNQuiz.Alice.Models
{
    public class QuizResponseBaseReturn
    {
        [JsonPropertyName("response")]
        public object? Response { get; set; }

        [JsonPropertyName("session_state")]
        public QuizSessionState? SessionState { get; set; }

        [JsonPropertyName("user_state_update")]
        public QuizUserState? UserStateUpdate { get; set; }

        [JsonPropertyName("application_state")]
        public QuizUserState? ApplicationState { get; set; }

        [JsonPropertyName("version")]
        public string? Version { get; set; }

        public static implicit operator QuizResponseBaseReturn(QuizResponseBase quizResponseBase)
        {
            return new QuizResponseBaseReturn()
            {
                Response = quizResponseBase.Response,
                SessionState = quizResponseBase.SessionState,
                UserStateUpdate = quizResponseBase.UserStateUpdate,
                ApplicationState = quizResponseBase.ApplicationState,
                Version = quizResponseBase.Version
            };
        }
    }

    public class QuizResponseBase : AliceResponseBase<AliceResponseModel, QuizSessionState, QuizUserState>
    {
        public QuizResponseBase()
        {
        }

        public QuizResponseBase(AliceRequestBase<QuizSessionState, QuizUserState> request, string text)
            : this(request, text, text, new List<AliceButtonModel>())
        {
        }

        public QuizResponseBase(AliceRequestBase<QuizSessionState, QuizUserState> request, string text, string tts, List<AliceButtonModel> buttons) 
            : base(request, text, tts, buttons)
        {
        }

        public static implicit operator QuizResponseBase(QuizGalleryResponse quizGalleryResponse)
        {
            return new QuizResponseBase()
            {
                Response = quizGalleryResponse.Response,
                SessionState = quizGalleryResponse.SessionState,
                UserStateUpdate = quizGalleryResponse.UserStateUpdate,
                ApplicationState = quizGalleryResponse.ApplicationState,
                Version = quizGalleryResponse.Version
            };
        }

        public static implicit operator QuizResponseBase(QuizResponse quizResponse)
        {
            return new QuizResponseBase()
            {
                Response = quizResponse.Response,
                SessionState = quizResponse.SessionState,
                UserStateUpdate = quizResponse.UserStateUpdate,
                ApplicationState = quizResponse.ApplicationState,
                Version = quizResponse.Version
            };
        }
    }

    public class QuizGalleryResponse : AliceGalleryResponse<QuizSessionState, QuizUserState>
    {
        public QuizGalleryResponse(AliceRequestBase<QuizSessionState, QuizUserState> request, string text, bool keepSessionState = true, bool keepUserState = true) : base(request, text, keepSessionState, keepUserState)
        {
        }

        public QuizGalleryResponse(AliceRequestBase<QuizSessionState, QuizUserState> request, string text, string tts, bool keepSessionState = true, bool keepUserState = true) : base(request, text, tts, keepSessionState, keepUserState)
        {
        }

        public QuizGalleryResponse(AliceRequestBase<QuizSessionState, QuizUserState> request, string text, List<AliceButtonModel> buttons, bool keepSessionState = true, bool keepUserState = true) : base(request, text, buttons, keepSessionState, keepUserState)
        {
        }

        public QuizGalleryResponse(AliceRequestBase<QuizSessionState, QuizUserState> request, string text, string tts, List<AliceButtonModel> buttons, bool keepSessionState = true, bool keepUserState = true) : base(request, text, tts, buttons, keepSessionState, keepUserState)
        {
        }
    }

    public class QuizResponse : AliceImageResponse<QuizSessionState, QuizUserState>
    {
        public QuizResponse(AliceRequestBase<QuizSessionState, QuizUserState> request, string text) : base(request, text)
        {
        }

        public QuizResponse(AliceRequestBase<QuizSessionState, QuizUserState> request, string text, string tts) : base(request, text, tts)
        {
        }

        public QuizResponse(AliceRequestBase<QuizSessionState, QuizUserState> request, string text, List<QuizButtonModel> buttons) : base(request, text, buttons.Cast<AliceButtonModel>().ToList())
        {
        }

        public QuizResponse(AliceRequestBase<QuizSessionState, QuizUserState> request, string text, string tts, List<QuizButtonModel> buttons) : base(request, text, tts, buttons.Cast<AliceButtonModel>().ToList())
        {
        }
    }
}
