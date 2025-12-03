using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASConfigurator.Core.Models;

namespace ASConfigurator.Core.RiskAnalysis
{
    public class RiskAnalyzer
    {
        public RiskSummary Analyze(IEnumerable<PolicySetting> policies)
        {
            var summary = new ConcurrentDictionary<string, int>();
            summary["Critical"] = 0;
            summary["High"] = 0;
            summary["Medium"] = 0;
            summary["Low"] = 0;

            Parallel.ForEach(policies, policy =>
            {
                summary.AddOrUpdate(policy.Risk, 1, (_, current) => current + 1);
            });

            return new RiskSummary
            {
                Critical = summary["Critical"],
                High = summary["High"],
                Medium = summary["Medium"],
                Low = summary["Low"]
            };
        }

        public IDictionary<string, string> GetRiskDescriptions()
        {
            return new Dictionary<string, string>
            {
                ["Firewall Off"] = "Критично: відключений брандмауер дозволяє зовнішні атаки.",
                ["SMBv1 On"] = "Критично: експлойти EternalBlue досі працюють на SMBv1.",
                ["RDP без NLA"] = "High: підвищує ризик credential harvesting.",
                ["BitLocker Off"] = "Critical: втрата фізичного носія призведе до витоку.",
                ["Guest Enabled"] = "High: обходження контролю доступу."
            };
        }
    }
}
