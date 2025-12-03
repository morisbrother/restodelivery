using System.Collections.Generic;
using System.Linq;
using ASConfigurator.Core.Models;

namespace ASConfigurator.Core.Compliance
{
    public class ComplianceService
    {
        public ComplianceScores Calculate(IEnumerable<SecurityProfile> profiles)
        {
            double as1 = CalculateFor(profiles, "АС-1");
            double as2 = CalculateFor(profiles, "АС-2");
            double as3 = CalculateFor(profiles, "АС-3");
            return new ComplianceScores { AS1 = as1, AS2 = as2, AS3 = as3 };
        }

        private double CalculateFor(IEnumerable<SecurityProfile> profiles, string level)
        {
            var profile = profiles.FirstOrDefault(p => p.Level == level);
            if (profile == null || !profile.Policies.Any())
            {
                return 0;
            }

            var completed = profile.Policies.Count(p => p.Status == "Виконано");
            return (double)completed / profile.Policies.Count * 100;
        }
    }
}
