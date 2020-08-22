using System;
using Microsoft.Extensions.Logging;

namespace DealerTrack.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly string _fileName;

        public FileLoggerProvider(Func<string, LogLevel, bool> filter, string fileName)
        {
            _filter = filter;
            _fileName = fileName;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName, _filter, _fileName);
        }

        public void Dispose() { }
    }
}
