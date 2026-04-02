using FluentAssertions;
using Microsoft.AspNetCore.Html;
using UContentMapper.Tests.Umbraco17.TestHelpers;
using UContentMapper.Umbraco17.Extensions;
using Umbraco.Cms.Core.Strings;

namespace UContentMapper.Tests.Umbraco17.Unit.Umbraco17.Extensions;

[TestFixture]
public class ContentExtensionsTests : TestBase
{
    [Test]
    public void ToHtmlContent_ShouldConvertHtmlEncodedString()
    {
        // Arrange
        var htmlContent = "<p>Test Content</p>";
        var htmlEncodedString = new HtmlEncodedString(htmlContent);

        // Act
        var result = htmlEncodedString.ToHtmlContent();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<HtmlString>();
        result.Should().BeAssignableTo<IHtmlContent>();
        ((HtmlString)result).Value.Should().Be(htmlContent);
    }

    [Test]
    public void ToHtmlContent_ShouldHandleEmptyString()
    {
        // Arrange
        var htmlEncodedString = new HtmlEncodedString(string.Empty);

        // Act
        var result = htmlEncodedString.ToHtmlContent();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<HtmlString>();
        ((HtmlString)result).Value.Should().BeEmpty();
    }

    [Test]
    public void ToHtmlContent_ShouldPreserveHtmlEncoding()
    {
        // Arrange
        var htmlContent = "&lt;p&gt;Test &amp; Content&lt;/p&gt;";
        var htmlEncodedString = new HtmlEncodedString(htmlContent);

        // Act
        var result = htmlEncodedString.ToHtmlContent();

        // Assert
        result.Should().NotBeNull();
        ((HtmlString)result).Value.Should().Be(htmlContent);
    }

    [Test]
    public void ToHtmlContent_ShouldHandleNull()
    {
        // Arrange
        var htmlEncodedString = new HtmlEncodedString(null!);

        // Act
        var result = htmlEncodedString.ToHtmlContent();

        // Assert
        result.Should().NotBeNull();
        ((HtmlString)result).Value.Should().BeNull();
    }
}
