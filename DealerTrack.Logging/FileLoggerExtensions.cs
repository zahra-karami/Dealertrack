using System;
using Microsoft.Extensions.Logging;

namespace DealerTrack.Logging
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder loggerBuilder, string filePath, Func<string, LogLevel, bool> filter)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var fileInfo = new System.IO.FileInfo(filePath);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            loggerBuilder.AddProvider(new FileLoggerProvider(filter, filePath));

            return loggerBuilder;
        }

        public static ILoggingBuilder AddFile(this ILoggingBuilder loggerBuilder, string filePath, LogLevel minimumLevel = LogLevel.Information)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var fileInfo = new System.IO.FileInfo(filePath);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            loggerBuilder.AddProvider(new FileLoggerProvider((category, logLevel) => logLevel >= minimumLevel, filePath));

            return loggerBuilder;
        }
    }
}
