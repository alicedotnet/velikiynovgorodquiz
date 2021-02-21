using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace VNQuiz.Alice.Tests.TestsInfrastructure.Fixtures
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private readonly IMessageSink _messageSink;

        public CustomWebApplicationFactory(IMessageSink messageSink)
        {
            _messageSink = messageSink;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Register the xUnit logger
            builder.ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.Services.AddSingleton<ILoggerProvider>(serviceProvider
                    => new XUnitLoggerProvider(_messageSink));
            })
                .ConfigureAppConfiguration(config =>
                {
                    var integrationConfig = new ConfigurationBuilder()
                        .AddJsonFile("integrationsettings.json")
                        .Build();

                    config.AddConfiguration(integrationConfig);
                });
        }
    }
}
