using System;
using Microsoft.Extensions.Logging;

namespace DealerTrack.Logging
{
    public sealed class FileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly FileLoggerHelper _helper;

        public FileLogger(string categoryName, Func<string, LogLevel, bool> filter, string fileName)
        {
            _categoryName = categoryName;
            _filter = filter;
            _helper = new FileLoggerHelper(fileName);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _filter == null || _filter(_categoryName, logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            var message = formatter(state, exception);

            if (string.IsNullOrEmpty(message)) return;

            if (exception != null) message += "\n" + exception.ToString();

            var logEntry = new LogEntry
            {
                Message = message,
                EventId = eventId.Id,
                LogLevel = logLevel.ToString(),
                CreatedTime = DateTime.UtcNow
            };

            _helper.InsertLog(logEntry);
        }
    }
}
