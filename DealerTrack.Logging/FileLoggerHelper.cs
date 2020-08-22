using System;
using System.Threading;

namespace DealerTrack.Logging
{
    public class FileLoggerHelper
    {
        private readonly string fileName;
        static readonly ReaderWriterLock locker = new ReaderWriterLock();

        public FileLoggerHelper(string fileName)
        {
            this.fileName = fileName;
        }
        
        public void InsertLog(LogEntry logEntry)
        {
            var directory = System.IO.Path.GetDirectoryName(fileName);

            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            try
            {
                locker.AcquireWriterLock(int.MaxValue);
                System.IO.File.AppendAllText(fileName, $"{logEntry.CreatedTime} {logEntry.EventId} { logEntry.LogLevel}  { logEntry.Message}  " + Environment.NewLine);
            }
            finally
            {
                locker.ReleaseWriterLock();
            }
        }
    }
}
