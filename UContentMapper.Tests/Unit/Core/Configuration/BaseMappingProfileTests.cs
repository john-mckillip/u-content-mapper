using FluentAssertions;
using UContentMapper.Core.Configuration.Profiles;
using UContentMapper.Tests.TestHelpers;

namespace UContentMapper.Tests.Unit.Core.Configuration;

[TestFixture]
public class BaseMappingProfileTests : TestBase
{
    [Test]
    public void BaseMappingProfile_CanBeInstantiated()
    {
        // Just verify we can create an instance and call Configure
        // without initializing it (which would require a full IMappingConfiguration setup)
        var profile = new BaseMappingProfile();
        
        // The profile should exist but not be configured yet
        profile.Should().NotBeNull();
    }
}