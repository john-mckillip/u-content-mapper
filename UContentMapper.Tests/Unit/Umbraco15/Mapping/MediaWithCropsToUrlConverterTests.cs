using FluentAssertions;
using System.Runtime.CompilerServices;
using UContentMapper.Tests.TestHelpers;
using UContentMapper.Umbraco15.Mapping;
using Umbraco.Cms.Core.Models;

namespace UContentMapper.Tests.Unit.Umbraco15.Mapping;

[TestFixture]
public class MediaWithCropsToUrlConverterTests : TestBase
{
    private static MediaWithCrops CreateNonNullMedia() =>
        (MediaWithCrops)RuntimeHelpers.GetUninitializedObject(typeof(MediaWithCrops));

    [Test]
    public void Convert_WhenSourceIsNull_ShouldReturnEmptyString()
    {
        var converter = new MediaWithCropsToUrlConverter(_ => "should-not-be-called");

        var result = converter.Convert(null!);

        result.Should().BeEmpty();
    }

    [Test]
    public void Convert_WhenUrlResolverReturnsValue_ShouldReturnValue()
    {
        var converter = new MediaWithCropsToUrlConverter(_ => "/media/image.jpg");

        var result = converter.Convert(CreateNonNullMedia());

        result.Should().Be("/media/image.jpg");
    }

    [Test]
    public void Convert_WhenUrlResolverReturnsNull_ShouldReturnEmptyString()
    {
        var converter = new MediaWithCropsToUrlConverter(_ => null);

        var result = converter.Convert(CreateNonNullMedia());

        result.Should().BeEmpty();
    }

    [Test]
    public void Convert_WhenUrlResolverThrowsMissingUrlProviderException_ShouldReturnEmptyString()
    {
        var converter = new MediaWithCropsToUrlConverter(_ => throw new InvalidOperationException("Unable to resolve IUrlProvider"));

        var result = converter.Convert(CreateNonNullMedia());

        result.Should().BeEmpty();
    }

    [Test]
    public void Convert_WhenUrlResolverThrowsUnexpectedException_ShouldPropagateException()
    {
        var converter = new MediaWithCropsToUrlConverter(_ => throw new InvalidOperationException("Some other failure"));

        var action = () => converter.Convert(CreateNonNullMedia());

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Some other failure");
    }
}