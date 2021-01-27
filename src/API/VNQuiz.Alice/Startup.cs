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

            var helper = new QuestionsHelper();
            helper.Initialize(Path.Combine(Environment.CurrentDirectory, "questions.json"));

            services.AddSingleton<IQuestionsHelper>(helper);
            services.AddScoped<IQuestionsService, QuestionsService>();
            services.AddScoped<IScenesProvider, ScenesProvider>();
            services.AddScoped<WelcomeScene>();
            services.AddScoped<StartGameScene>();
            services.AddScoped<QuestionScene>();
            services.AddScoped<CorrectAnswerScene>();
            services.AddScoped<WrongAnswerScene>();
            services.AddScoped<EndGameScene>();
            services.AddScoped<EndSessionScene>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VNQuiz.Alice", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VNQuiz.Alice v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
