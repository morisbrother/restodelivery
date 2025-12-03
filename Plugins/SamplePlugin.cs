using System.Collections.Generic;
using ASConfigurator.Core.Models;

namespace ASConfigurator.Plugins
{
    public class SamplePlugin : IConfiguratorPlugin
    {
        public string Name => "Sample plugin";
        public string TabName => "Plugins";

        public IEnumerable<PolicySetting> ExtendPolicies()
        {
            return new List<PolicySetting>
            {
                new PolicySetting
                {
                    Name = "Plugin Policy",
                    ProfileLevel = "АС-2",
                    Risk = "Low",
                    Status = "Не застосовано",
                    Explanation = "Розширення політик через плагін"
                }
            };
        }
    }
}
