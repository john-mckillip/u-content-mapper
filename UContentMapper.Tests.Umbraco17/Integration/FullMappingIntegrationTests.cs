using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using UContentMapper.Core.Abstractions.Mapping;
using UContentMapper.Tests.Umbraco17.Fixtures;
using UContentMapper.Tests.Umbraco17.Mocks;
using UContentMapper.Tests.Umbraco17.TestHelpers;
using UContentMapper.Umbraco17.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace UContentMapper.Tests.Umbraco17.Integration;

[TestFixture]
public class FullMappingIntegrationTests : TestBase
{
    private IServiceProvider _serviceProvider;

    [SetUp]
    public override void SetUp()
    {
        base.SetUp();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddUContentMapper();

        _serviceProvider = services.BuildServiceProvider();
    }

    [TearDown]
    public override void TearDown()
    {
        base.TearDown();
        (_serviceProvider as IDisposable)?.Dispose();
    }

    [Test]
    public void EndToEndMapping_SimpleModel_ShouldMapSuccessfully()
    {
        // Arrange
        var mapper = _serviceProvider.GetRequiredService<IContentMapper<TestPageModel>>();
        var content = TestDataBuilder.CreatePublishedContentWithBuiltInProperties();

        // Act
        var canMap = mapper.CanMap(content);
        var result = mapper.Map(content);

        // Assert
        canMap.Should().BeTrue();
        result.Should().NotBeNull();
        result.Id.Should().Be(1001);
        result.Key.Should().Be(new Guid("12345678-1234-1234-1234-123456789012"));
        result.Name.Should().Be("Test Content Name");
        result.ContentTypeAlias.Should().Be("testPage");
        result.Level.Should().Be(2);
        result.SortOrder.Should().Be(5);
        result.TemplateId.Should().Be(9999);
    }

    [Test]
    public void EndToEndMapping_CanMap_ShouldReturnCorrectValue()
    {
        var mapper = _serviceProvider.GetRequiredService<IContentMapper<TestPageModel>>();

        // IPublishedContent
        var content = TestDataBuilder.CreatePublishedContentWithBuiltInProperties();

        var canMap = mapper.CanMap(content);

        canMap.Should().BeTrue();

        // Get IPublishedContent with empty string for content type alias
        content = TestDataBuilder.CreatePublishedContentWithEmptyStringContentTypeAlias();

        canMap = mapper.CanMap(content);

        canMap.Should().BeFalse();

        // IPublishedElement

        var element = TestDataBuilder.CreatePublishedElementWithEmptyStringContentTypeAlias();

        canMap = mapper.CanMap(element);

        canMap.Should().BeFalse();
    }

    [Test]
    public void EndToEndMapping_WithCustomProperties_ShouldMapSuccessfully()
    {
        // Arrange
        var mapper = _serviceProvider.GetRequiredService<IContentMapper<TestPageModel>>();

        // Create content mock with properties
        var mock = MockPublishedContent.Create();
        var publishedPropertyTypeMock = new Mock<IPublishedPropertyType>();

        // Set up content type alias
        var contentTypeMock = new Mock<IPublishedContentType>();
        contentTypeMock.Setup(x => x.Alias).Returns("testPage");
        mock.Setup(x => x.ContentType).Returns(contentTypeMock.Object);

        // Set up properties
        var properties = new Dictionary<string, object>
        {
            { "title", "Integration Test Title" },
            { "mainbody", new HtmlEncodedString("{<p>This is some test html from a WYSIWYG editor.</p>}") },
            { "description", "Integration test description" },
            { "categoryid", 999 },
            { "ispublished", true },
            { "publishdate", DateTime.UtcNow.AddDays(-5) }
        };

        foreach (var prop in properties)
        {
            var propertyMock = new Mock<IPublishedProperty>();
            propertyMock.Setup(x => x.Alias).Returns(prop.Key);
            propertyMock.Setup(x => x.HasValue(It.IsAny<string>(), It.IsAny<string>())).Returns(prop.Value is not null);
            propertyMock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns(prop.Value);

            mock.Setup(x => x.GetProperty(prop.Key)).Returns(propertyMock.Object);
            mock.Setup(x => x.ContentType.GetPropertyType(prop.Key)).Returns(publishedPropertyTypeMock.Object);
        }

        // Act
        var result = mapper.Map(mock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Integration Test Title");
        result.Description.Should().Be("Integration test description");
        result.MainBody.Should().NotBe(null);
        result.MainBody?.ToString().Should().Be("{<p>This is some test html from a WYSIWYG editor.</p>}");
        result.CategoryId.Should().Be(999);
        result.IsPublished.Should().BeTrue();
        result.PublishDate.Should().BeCloseTo(DateTime.UtcNow.AddDays(-5), TimeSpan.FromSeconds(1));
    }

    [Test]
    public void EndToEndMapping_WhenMappedPropertyExists_ShouldResolveStringValue()
    {
        // Arrange
        var mapper = _serviceProvider.GetRequiredService<IContentMapper<TestPageModel>>();
        var publishedPropertyTypeMock = new Mock<IPublishedPropertyType>();
        var content = MockPublishedContent.Create();

        var contentTypeMock = new Mock<IPublishedContentType>();
        contentTypeMock.Setup(x => x.Alias).Returns("testPage");
        content.Setup(x => x.ContentType).Returns(contentTypeMock.Object);
        content.Setup(x => x.ContentType.GetPropertyType("title")).Returns(publishedPropertyTypeMock.Object);

        var titlePropertyMock = new Mock<IPublishedProperty>();
        titlePropertyMock.Setup(x => x.Alias).Returns("title");
        titlePropertyMock.Setup(x => x.HasValue(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        titlePropertyMock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns("Resolved By Integration");
        content.Setup(x => x.GetProperty("title")).Returns(titlePropertyMock.Object);

        // Act
        var result = mapper.Map(content.Object);

        // Assert
        result.Title.Should().Be("Resolved By Integration");
    }

    [Test]
    public void EndToEndMapping_WhenMappedPropertyIsMissing_ShouldKeepDefaultStringValue()
    {
        // Arrange
        var mapper = _serviceProvider.GetRequiredService<IContentMapper<TestPageModel>>();
        var content = MockPublishedContent.Create();

        var contentTypeMock = new Mock<IPublishedContentType>();
        contentTypeMock.Setup(x => x.Alias).Returns("testPage");
        content.Setup(x => x.ContentType).Returns(contentTypeMock.Object);

        // Act
        var result = mapper.Map(content.Object);

        // Assert
        result.Title.Should().BeEmpty();
    }

    [Test]
    public void EndToEndMapping_TypeConversion_ShouldMapSuccessfully()
    {
        // Arrange
        var mapper = _serviceProvider.GetRequiredService<IContentMapper<TypeConversionTestModel>>();

        // Create content mock with properties
        var mock = MockPublishedContent.Create();
        var publishedPropertyTypeMock = new Mock<IPublishedPropertyType>();

        // Set up content type alias
        var contentTypeMock = new Mock<IPublishedContentType>();
        contentTypeMock.Setup(x => x.Alias).Returns("typeConversionTest");
        mock.Setup(x => x.ContentType).Returns(contentTypeMock.Object);

        // Set up properties
        var properties = new Dictionary<string, object>
        {
            { "stringvalue", "Test String Value" },
            { "intvalue", 42 },
            { "boolvalue", true },
            { "datetimevalue", new DateTime(2026, 1, 1, 10, 30, 0, DateTimeKind.Utc) },
            { "guidvalue", "12345678-1234-1234-1234-123456789012" },
            { "doublevalue", 3.14159 },
            { "decimalvalue", 999.99m },
            { "floatvalue", 2.718F },
            { "longvalue", 9223372036854775807 },
            { "shortvalue", 32767 },
            { "nullablehtmlcontentvalue", new HtmlEncodedString("{<p>This is some test html from a WYSIWYG editor.</p>}")}
        };

        foreach (var prop in properties)
        {
            var propertyMock = new Mock<IPublishedProperty>();
            propertyMock.Setup(x => x.Alias).Returns(prop.Key);
            propertyMock.Setup(x => x.HasValue(It.IsAny<string>(), It.IsAny<string>())).Returns(prop.Value != null);
            propertyMock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns(prop.Value);

            mock.Setup(x => x.GetProperty(prop.Key)).Returns(propertyMock.Object);
            mock.Setup(x => x.ContentType.GetPropertyType(prop.Key)).Returns(publishedPropertyTypeMock.Object);
        }

        // Act
        var result = mapper.Map(mock.Object);

        // Assert
        result.Should().NotBeNull();
        result.StringValue.Should().Be("Test String Value");
        result.IntValue.Should().Be(42);
        result.BoolValue.Should().BeTrue();
        result.DateTimeValue.Should().Be(new DateTime(2026, 1, 1, 10, 30, 0, DateTimeKind.Utc));
        result.GuidValue.Should().Be(new Guid("12345678-1234-1234-1234-123456789012"));
        result.DoubleValue.Should().BeApproximately(3.14159, 0.00001);
        result.DecimalValue.Should().Be(999.99m);
        result.FloatValue.Should().BeApproximately(2.718f, 0.001f);
        result.LongValue.Should().Be(9223372036854775807);
        result.ShortValue.Should().Be(32767);
        result.NullableHtmlContentValue.Should().NotBe(null);
        result.NullableHtmlContentValue?.ToString().Should().Be("{<p>This is some test html from a WYSIWYG editor.</p>}");
    }

    [Test]
    public void EndToEndMapping_WildcardContentType_ShouldMapSuccessfully()
    {
        // Arrange
        var mapper = _serviceProvider.GetRequiredService<IContentMapper<WildcardContentTypeModel>>();

        // Create content mock with properties
        var mock = MockPublishedContent.Create();
        var publishedPropertyTypeMock = new Mock<IPublishedPropertyType>();

        // Set up content type alias
        var contentTypeMock = new Mock<IPublishedContentType>();
        contentTypeMock.Setup(x => x.Alias).Returns("anyContentType");
        mock.Setup(x => x.ContentType).Returns(contentTypeMock.Object);

        // Set up custom property
        var propertyMock = new Mock<IPublishedProperty>();
        propertyMock.Setup(x => x.Alias).Returns("customproperty");
        propertyMock.Setup(x => x.HasValue(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        propertyMock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns("Custom Value");

        mock.Setup(x => x.GetProperty("customproperty")).Returns(propertyMock.Object);
        mock.Setup(x => x.ContentType.GetPropertyType("customproperty")).Returns(publishedPropertyTypeMock.Object);

        // Act
        var canMap = mapper.CanMap(mock.Object);
        var result = mapper.Map(mock.Object);

        // Assert
        canMap.Should().BeTrue();
        result.Should().NotBeNull();
        result.CustomProperty.Should().Be("Custom Value");
    }

    [Test]
    public void EndToEndMapping_InvalidContentType_ShouldNotMap()
    {
        // Arrange
        var mapper = _serviceProvider.GetRequiredService<IContentMapper<TestPageModel>>();

        // Create content mock with properties
        var mock = MockPublishedContent.Create();
        var contentTypeMock = new Mock<IPublishedContentType>();
        contentTypeMock.Setup(x => x.Alias).Returns("wrongContentType");
        mock.Setup(x => x.ContentType).Returns(contentTypeMock.Object);

        // Act
        var canMap = mapper.CanMap(mock.Object);

        // Assert
        canMap.Should().BeFalse();
    }

    [Test]
    public void EndToEndMapping_InvalidSource_ShouldThrowException()
    {
        // Arrange
        var mapper = _serviceProvider.GetRequiredService<IContentMapper<TestPageModel>>();
        var invalidSource = new object();

        // Act
        var canMap = mapper.CanMap(invalidSource);
        var act = () => mapper.Map(invalidSource);

        // Assert
        canMap.Should().BeFalse();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot map object of type Object to TestPageModel");
    }

    [Test]
    public void EndToEndMapping_NullAndEmptyValues_ShouldHandleGracefully()
    {
        // Arrange
        var mapper = _serviceProvider.GetRequiredService<IContentMapper<TestPageModel>>();

        // Create content mock with properties
        var mock = MockPublishedContent.Create();
        // Set up content type alias
        var contentTypeMock = new Mock<IPublishedContentType>();
        contentTypeMock.Setup(x => x.Alias).Returns("testPage");
        mock.Setup(x => x.ContentType).Returns(contentTypeMock.Object);

        // Set up properties with null/empty values
        var properties = new Dictionary<string, object?>
        {
            { "title", null! },
            { "description", "" },
            { "categoryid", null! },
            { "ispublished", null! }
        };

        foreach (var prop in properties)
        {
            var propertyMock = new Mock<IPublishedProperty>();
            propertyMock.Setup(x => x.Alias).Returns(prop.Key);
            propertyMock.Setup(x => x.HasValue(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            propertyMock.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns(prop.Value);

            mock.Setup(x => x.GetProperty(prop.Key)).Returns(propertyMock.Object);
        }

        // Act
        var result = mapper.Map(mock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(string.Empty);
        result.Description.Should().Be("");
        result.CategoryId.Should().Be(0);
        result.IsPublished.Should().BeFalse();
    }
}
