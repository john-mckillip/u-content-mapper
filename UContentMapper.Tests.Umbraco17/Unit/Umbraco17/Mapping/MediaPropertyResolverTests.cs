using FluentAssertions;
using Moq;
using NUnit.Framework;
using UContentMapper.Tests.Umbraco17.Mocks;
using UContentMapper.Tests.Umbraco17.TestHelpers;
using UContentMapper.Umbraco17.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UContentMapper.Tests.Umbraco17.Unit.Umbraco17.Mapping;

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
    public void Resolve_WhenPropertyExistsAndUmbracoValueFallbackIsUnavailable_ShouldThrowTypeInitializationException()
    {
        var resolver = new MediaPropertyResolver<string>("title");
        var contentMock = MockPublishedContent.Create();
        var publishedPropertyTypeMock = new Mock<IPublishedPropertyType>();
        contentMock.Setup(x => x.ContentType.GetPropertyType("title")).Returns(publishedPropertyTypeMock.Object);

        var action = () => resolver.Resolve(contentMock.Object);

        action.Should().Throw<TypeInitializationException>();
    }
}
