using System.Collections.Generic;

namespace ASConfigurator.Core.Models
{
    public class SecurityProfile
    {
        public string Name { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IList<PolicySetting> Policies { get; set; } = new List<PolicySetting>();
        public IList<string> OrganizationalRequirements { get; set; } = new List<string>();
        public IDictionary<string, string> TechnicalRequirements { get; set; } = new Dictionary<string, string>();
    }
}
