using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace ASConfigurator.Infrastructure.Logging
{
    public class LogService
    {
        private readonly ConcurrentQueue<string> _entries = new();
        private readonly string _logPath;

        public LogService(string? logPath = null)
        {
            _logPath = logPath ?? Path.Combine("Logs", "asconfig.log");
            Directory.CreateDirectory(Path.GetDirectoryName(_logPath)!);
        }

        public void Info(string message)
        {
            var entry = $"[{DateTime.UtcNow:u}] {message}";
            _entries.Enqueue(entry);
        }

        public async Task FlushAsync()
        {
            using var writer = new StreamWriter(_logPath, append: true);
            while (_entries.TryDequeue(out var entry))
            {
                await writer.WriteLineAsync(entry);
            }
        }
    }
}
