using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ASConfigurator.Infrastructure.INI
{
    public class IniParserService
    {
        public async Task<IDictionary<string, string>> ParseAsync(string path)
        {
            var dict = new Dictionary<string, string>();
            if (!File.Exists(path))
            {
                return dict;
            }

            var lines = await File.ReadAllLinesAsync(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    continue;
                }

                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    dict[parts[0].Trim()] = parts[1].Trim();
                }
            }

            return dict;
        }
    }
}
