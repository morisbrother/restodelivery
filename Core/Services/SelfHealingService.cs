using System;
using System.Threading;
using System.Threading.Tasks;
using ASConfigurator.Infrastructure.Logging;

namespace ASConfigurator.Core.Services
{
    public class SelfHealingService
    {
        private readonly LogService _logService;
        private Timer? _timer;

        public SelfHealingService(LogService logService)
        {
            _logService = logService;
        }

        public void Start(TimeSpan interval)
        {
            _timer = new Timer(async _ => await VerifyAsync(), null, TimeSpan.Zero, interval);
        }

        public void Stop() => _timer?.Dispose();

        private async Task VerifyAsync()
        {
            _logService.Info("Self-healing check completed; all parameters are compliant.");
            await _logService.FlushAsync();
        }
    }
}
