using FluentAssertions;
using UContentMapper.Core.Mapping;
using UContentMapper.Tests.TestHelpers;

namespace UContentMapper.Tests.Unit.Core.Mapping;

[TestFixture]
public class BasicPropertyValueConverterTests : TestBase
{
    private BasicPropertyValueConverter _converter;

    [SetUp]
    public override void SetUp()
    {
        base.SetUp();
        _converter = new BasicPropertyValueConverter();
    }

    [Test]
    public void CanConvert_NullValue_ShouldReturnTrue()
    {
        _converter.CanConvert(null, typeof(int)).Should().BeTrue();
    }

    [Test]
    public void CanConvert_AssignableType_ShouldReturnTrue()
    {
        _converter.CanConvert("test", typeof(string)).Should().BeTrue();
    }

    [Test]
    public void CanConvert_UnsupportedType_ShouldReturnFalse()
    {
        _converter.CanConvert(1.23m, typeof(decimal)).Should().BeFalse();
    }

    [Test]
    public void ConvertValue_NullToReferenceType_ShouldReturnNull()
    {
        _converter.ConvertValue(null, typeof(string)).Should().BeNull();
    }

    [Test]
    public void ConvertValue_NullToValueType_ShouldReturnDefault()
    {
        var result = _converter.ConvertValue(null, typeof(int));
        result.Should().Be(0);
    }

    [Test]
    public void ConvertValue_StringToInt_Valid_ShouldReturnParsedInt()
    {
        var result = _converter.ConvertValue("42", typeof(int));
        result.Should().Be(42);
    }

    [Test]
    public void ConvertValue_StringToInt_Invalid_ShouldReturnOriginalValue()
    {
        var result = _converter.ConvertValue("abc", typeof(int));
        result.Should().Be("abc");
    }

    [Test]
    public void ConvertValue_StringToBool_Valid_ShouldReturnParsedBool()
    {
        var result = _converter.ConvertValue("true", typeof(bool));
        result.Should().Be(true);
    }

    [Test]
    public void ConvertValue_StringToBool_Invalid_ShouldReturnOriginalValue()
    {
        var result = _converter.ConvertValue("notbool", typeof(bool));
        result.Should().Be("notbool");
    }

    [Test]
    public void ConvertValue_StringToGuid_Valid_ShouldReturnGuid()
    {
        var guid = Guid.NewGuid();
        var result = _converter.ConvertValue(guid.ToString(), typeof(Guid));
        result.Should().Be(guid);
    }

    [Test]
    public void ConvertValue_StringToGuid_Invalid_ShouldReturnOriginalValue()
    {
        var result = _converter.ConvertValue("notguid", typeof(Guid));
        result.Should().Be("notguid");
    }
}