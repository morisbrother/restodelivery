using System.Collections.Generic;

namespace ASConfigurator.Infrastructure.Security
{
    public class SecurityScanner
    {
        public IList<string> DetectDangerousSoftware()
        {
            // Simulated detection
            return new List<string> { "TeamViewer", "AnyDesk", "Radmin" };
        }
    }
}
