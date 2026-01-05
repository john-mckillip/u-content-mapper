using FluentAssertions;
using NUnit.Framework;
using UContentMapper.Tests.Mocks;
using UContentMapper.Tests.TestHelpers;
using UContentMapper.Umbraco15.Mapping;

namespace UContentMapper.Tests.Unit.Umbraco15.Mapping;

[TestFixture]
public class MediaWithCropsToUrlConverterTests : TestBase
{
    [Test]
    public void Convert_WhenSourceIsNull_ShouldReturnEmptyString()
    {
        var converter = new MediaWithCropsToUrlConverter();

        var result = converter.Convert(null!);

        result.Should().BeEmpty();
    }

    [Test]
    public void Convert_WithNullMediaWithCrops_ShouldReturnEmptyString()
    {
        var converter = new MediaWithCropsToUrlConverter();
        var media = MockMediaWithCrops.Create();

        // media will be null since MediaWithCrops can't be easily mocked
        var result = converter.Convert(media!);

        result.Should().BeEmpty();
    }
}