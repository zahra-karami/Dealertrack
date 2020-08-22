using System;
using DealerTrack.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DealerTrack.Web.Extensions
{
    public static class ConfigureLoggingExtension
    {
        public static ILoggingBuilder AddLoggingConfiguration(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
        {
            var loggingOptions = new Options.LoggingOptions();
            configuration.GetSection("Logging").Bind(loggingOptions);

            foreach (var provider in loggingOptions.Providers)
            {
                switch (provider.Name.ToLower())
                {
                    case "console":
                        {
                            loggingBuilder.AddConsole();
                            break;
                        }
                    case "file":
                        {
                            var logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), provider.LogLevel);
                            string filePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "logs", $"DealerTrack_{System.DateTime.Now:ddMMyyHHmm}.log");
                            loggingBuilder.AddFile(filePath, logLevel);
                            break;
                        }
                }
            }

            return loggingBuilder;
        }
    }


}