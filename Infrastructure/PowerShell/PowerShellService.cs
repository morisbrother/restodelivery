using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASConfigurator.Infrastructure.PowerShell
{
    public class PowerShellService
    {
        public Task<IList<string>> RunAsync(IEnumerable<string> scripts)
        {
            // In training mode we do not execute anything; return mocked output
            return Task.FromResult<IList<string>>(new List<string> { "Simulated PowerShell execution" });
        }
    }
}
