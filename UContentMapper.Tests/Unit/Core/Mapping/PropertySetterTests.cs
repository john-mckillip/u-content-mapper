using FluentAssertions;
using Moq;
using UContentMapper.Core.Abstractions.Mapping;
using UContentMapper.Core.Mapping;
using UContentMapper.Tests.TestHelpers;

namespace UContentMapper.Tests.Unit.Core.Mapping;

[TestFixture]
public class PropertySetterTests : TestBase
{
    private Mock<IPropertyValueConverter> _converterMock;
    private PropertySetter _setter;

    [SetUp]
    public override void SetUp()
    {
        base.SetUp();
        _converterMock = CreateMock<IPropertyValueConverter>();
        _setter = new PropertySetter(_converterMock.Object);
    }

    private class TestModel
    {
        public string? StringProperty { get; set; }
        public int IntProperty { get; set; }
    }

    [Test]
    public void SetPropertyValue_WhenValueIsNull_ShouldNotCallConverterOrSetProperty()
    {
        var model = new TestModel();
        var property = typeof(TestModel).GetProperty(nameof(TestModel.StringProperty))!;

        _setter.SetPropertyValue(model, property, null);

        _converterMock.Verify(x => x.CanConvert(It.IsAny<object?>(), It.IsAny<Type>()), Times.Never);
        _converterMock.Verify(x => x.ConvertValue(It.IsAny<object?>(), It.IsAny<Type>()), Times.Never);
        model.StringProperty.Should().BeNull();
    }

    [Test]
    public void SetPropertyValue_WhenConverterCannotConvert_ShouldNotSetProperty()
    {
        var model = new TestModel();
        var property = typeof(TestModel).GetProperty(nameof(TestModel.IntProperty))!;

        _converterMock
            .Setup(x => x.CanConvert("value", typeof(int)))
            .Returns(false);

        _setter.SetPropertyValue(model, property, "value");

        _converterMock.Verify(x => x.CanConvert("value", typeof(int)), Times.Once);
        _converterMock.Verify(x => x.ConvertValue(It.IsAny<object?>(), It.IsAny<Type>()), Times.Never);
        model.IntProperty.Should().Be(0);
    }

    [Test]
    public void SetPropertyValue_WhenConverterCanConvert_ShouldSetConvertedValue()
    {
        var model = new TestModel();
        var property = typeof(TestModel).GetProperty(nameof(TestModel.IntProperty))!;

        _converterMock
            .Setup(x => x.CanConvert("42", typeof(int)))
            .Returns(true);

        _converterMock
            .Setup(x => x.ConvertValue("42", typeof(int)))
            .Returns(42);

        _setter.SetPropertyValue(model, property, "42");

        _converterMock.Verify(x => x.CanConvert("42", typeof(int)), Times.Once);
        _converterMock.Verify(x => x.ConvertValue("42", typeof(int)), Times.Once);
        model.IntProperty.Should().Be(42);
    }
}