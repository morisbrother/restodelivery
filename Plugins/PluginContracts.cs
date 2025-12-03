using System.Collections.Generic;
using ASConfigurator.Core.Models;

namespace ASConfigurator.Plugins
{
    public interface IConfiguratorPlugin
    {
        string Name { get; }
        IEnumerable<PolicySetting> ExtendPolicies();
        string TabName { get; }
    }
}
