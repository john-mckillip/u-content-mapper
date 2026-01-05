using UContentMapper.Core.Configuration.Profiles;
using UContentMapper.Tests.TestHelpers;

namespace UContentMapper.Tests.Unit.Core.Configuration;

[TestFixture]
public class BaseMappingProfileTests : TestBase
{
    private BaseMappingProfile _profile;

    [SetUp]
    public override void SetUp()
    {
        base.SetUp();
        _profile = new BaseMappingProfile();
    }

    [Test]
    public void Configure_ShouldBeIdempotent()
    {
        _profile.Configure();
        _profile.Configure();
        Assert.Pass();
    }
}