namespace ASConfigurator.Core.Models
{
    public class PolicySetting
    {
        public string Name { get; set; } = string.Empty;
        public string ProfileLevel { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Risk { get; set; } = string.Empty;
        public bool IsGpoControlled { get; set; }
        public string GpoName { get; set; } = string.Empty;
        public string Compliance { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
    }
}
