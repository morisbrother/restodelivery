using System.Collections.Generic;
using System.Linq;
using ASConfigurator.Core.Models;

namespace ASConfigurator.Core.Services
{
    public class PolicyService
    {
        public IList<PolicySetting> MergeWithRuntime(IList<PolicySetting> policies, IDictionary<string, string> runtimeStatus)
        {
            foreach (var policy in policies)
            {
                if (runtimeStatus.TryGetValue(policy.Name, out var status))
                {
                    policy.Status = status;
                }
                else
                {
                    policy.Status = "Unknown";
                }
            }

            return policies;
        }

        public IDictionary<string, string> GetTrainingExplanations(IList<PolicySetting> policies)
        {
            return policies.ToDictionary(p => p.Name, p => p.Explanation);
        }
    }
}
