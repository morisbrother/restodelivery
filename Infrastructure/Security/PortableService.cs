using System.IO;

namespace ASConfigurator.Infrastructure.Security
{
    public class PortableService
    {
        public bool IsPortable() => File.Exists("portable.mode");
    }
}
