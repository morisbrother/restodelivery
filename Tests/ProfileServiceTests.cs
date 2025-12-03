using ASConfigurator.Core.Services;
using Xunit;

namespace ASConfigurator.Tests
{
    public class ProfileServiceTests
    {
        [Fact]
        public void LoadsProfilesFromConfig()
        {
            var service = new ProfileService("Config/Profiles");
            var profiles = service.LoadProfiles();
            Assert.NotEmpty(profiles);
        }
    }
}
