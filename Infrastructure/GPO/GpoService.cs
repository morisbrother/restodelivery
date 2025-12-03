using System.Collections.Generic;

namespace ASConfigurator.Infrastructure.GPO
{
    public class GpoService
    {
        public IDictionary<string, string> DetectControlledPolicies()
        {
            // Simulated gpresult /v parsing and WMI queries
            return new Dictionary<string, string>
            {
                {"PasswordHistory", "SecurityBaseline"},
                {"Firewall Domain", "Corp Firewall"}
            };
        }
    }
}
