using UContentMapper.Tests.Umbraco17.Mocks;
using UContentMapper.Tests.Umbraco17.TestHelpers;
using UContentMapper.Umbraco17.Mapping;

namespace UContentMapper.Tests.Umbraco17.Unit.Umbraco17.Mapping;

[TestFixture]
public class PublishedContentToUrlConverterTests : TestBase
{
    [Test]
    public void Convert_WhenSourceIsNull_ShouldReturnEmptyString()
    {
        var converter = new PublishedContentToUrlConverter();

        var result = converter.Convert(null!);

        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Convert_WhenSourceIsNotNullAndUrlProviderIsUnavailable_ShouldThrowTypeInitializationException()
    {
        var converter = new PublishedContentToUrlConverter();
        var content = MockPublishedContent.Create().Object;

        Assert.Throws<TypeInitializationException>(() => converter.Convert(content));
    }
}
