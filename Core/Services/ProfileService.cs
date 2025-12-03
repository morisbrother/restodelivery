using System.Collections.Generic;
using System.IO;
using ASConfigurator.Core.Models;
using Newtonsoft.Json;

namespace ASConfigurator.Core.Services
{
    public class ProfileService
    {
        private readonly string _configFolder;

        public ProfileService(string? configFolder = null)
        {
            _configFolder = configFolder ?? Path.Combine("Config", "Profiles");
        }

        public IList<SecurityProfile> LoadProfiles()
        {
            var profiles = new List<SecurityProfile>();
            if (!Directory.Exists(_configFolder))
            {
                return profiles;
            }

            foreach (var file in Directory.GetFiles(_configFolder, "*.json"))
            {
                var json = File.ReadAllText(file);
                var profile = JsonConvert.DeserializeObject<SecurityProfile>(json);
                if (profile != null)
                {
                    profiles.Add(profile);
                }
            }

            return profiles;
        }
    }
}
