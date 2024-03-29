using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VNQuiz.Alice.Achievements;
using VNQuiz.Alice.Models;
using VNQuiz.Alice.Scenes;
using VNQuiz.Alice.Services;
using VNQuiz.Core;
using VNQuiz.Core.Interfaces;

namespace VNQuiz.Alice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers();

            services.AddSingleton<IQuestionsHelper, QuestionsHelper>();
            services.AddSingleton<IAchievementsHelper, AchievementsHelper>();

            services.AddScoped<IQuestionsService, QuestionsService>();
            services.AddScoped<IAchievementsService, AchievementsService>();

            services.AddScoped<IScenesProvider, ScenesProvider>();
            services.AddScoped<WelcomeScene>();
            services.AddScoped<StartGameScene>();
            services.AddScoped<QuestionScene>();
            services.AddScoped<CorrectAnswerScene>();
            services.AddScoped<WrongAnswerScene>();
            services.AddScoped<AdditionalInfoScene>();
            services.AddScoped<WinGameScene>();
            services.AddScoped<LoseGameScene>();
            services.AddScoped<RequestEndSessionScene>();
            services.AddScoped<RequestRestartScene>();
            services.AddScoped<RulesScene>();
            services.AddScoped<ProgressScene>();
            services.AddScoped<EndSessionScene>();

            services.AddScoped<FirstGameAchievementUnlocker>();
            services.AddScoped<CuriousAchievementUnlocker>();
            services.AddScoped<ThreeConsecutiveAnswersAchievementUnlocker>();
            services.AddScoped<FiveConsecutiveAnswersAchievementUnlocker>();
            services.AddScoped<SevenConsecutiveAnswersAchievementUnlocker>();
            services.AddScoped<TenConsecutiveAnswersAchievementUnlocker>();
            services.AddScoped<WinGameAchievementUnlocker>();

            var quizSettingsSection = Configuration.GetSection("QuizSettings");
            var quizSettings = new QuizSettings();
            quizSettingsSection.Bind(quizSettings);
            services.AddSingleton(quizSettings);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VNQuiz.Alice", Version = "v1" });
            });
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env
            , IQuestionsHelper questionsHelper, IAchievementsHelper achievementsHelper)
        {
            int questions = questionsHelper.Initialize(Path.Combine(Environment.CurrentDirectory, "questions.json"));
            Settings.AnsweredQuestionsToKeep = (int) (questions * 0.7);
            achievementsHelper.Initialize(Path.Combine(Environment.CurrentDirectory, "achievements.json"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VNQuiz.Alice v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<HandleFailedRequestsMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
