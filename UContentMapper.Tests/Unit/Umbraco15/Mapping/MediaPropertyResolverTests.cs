using FluentAssertions;
using NUnit.Framework;
using UContentMapper.Tests.Mocks;
using UContentMapper.Tests.TestHelpers;
using UContentMapper.Umbraco15.Mapping;

namespace UContentMapper.Tests.Unit.Umbraco15.Mapping;

[TestFixture]
public class MediaPropertyResolverTests : TestBase
{
    [Test]
    public void Resolve_WhenSourceIsNull_ShouldReturnDefault()
    {
        var resolver = new MediaPropertyResolver<string>("title");

        var result = resolver.Resolve(null!);

        result.Should().BeNull();
    }

    [Test]
    public void Resolve_WhenPropertyDoesNotExist_ShouldReturnDefault()
    {
        var resolver = new MediaPropertyResolver<string>("missing");
        var content = MockPublishedContent.Create().Object;

        var result = resolver.Resolve(content);

        result.Should().BeNull();
    }

    [Test]
    public void Resolve_WhenPropertyExists_ShouldReturnDefault_WhenNotConfigured()
    {
        var resolver = new MediaPropertyResolver<string>("title");
        var content = MockPublishedContent.Create().Object;

        var result = resolver.Resolve(content);

        result.Should().BeNull();
    }
}