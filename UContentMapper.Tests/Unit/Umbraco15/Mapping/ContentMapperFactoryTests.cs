using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using UContentMapper.Core.Abstractions.Mapping;
using UContentMapper.Umbraco15.Mapping;
using UContentMapper.Tests.TestHelpers;

namespace UContentMapper.Tests.Unit.Umbraco15.Mapping;

[TestFixture]
public class ContentMapperFactoryTests : TestBase
{
    private ContentMapperFactory _factory;

    private class TestModel { }

    private class TestContentMapper<TModel> : IContentMapper<TModel> where TModel : class, new()
    {
        public bool MapCalled { get; private set; }

        public bool CanMap(object source) => source is TModel;

        public TModel Map(object source)
        {
            MapCalled = true;
            return new TModel();
        }
    }

    [SetUp]
    public override void SetUp()
    {
        base.SetUp();
        Services.AddTransient(typeof(IContentMapper<>), typeof(TestContentMapper<>));
        var provider = BuildServiceProvider();
        _factory = new ContentMapperFactory(provider);
    }

    [Test]
    public void CreateMapper_Generic_ShouldResolveFromServiceProvider()
    {
        var mapper = _factory.CreateMapper<TestModel>();

        mapper.Should().NotBeNull();
        mapper.Should().BeOfType<TestContentMapper<TestModel>>();
    }

    [Test]
    public void CreateMapperForType_ShouldResolveFromServiceProvider()
    {
        var mapper = _factory.CreateMapperForType(typeof(TestModel));

        mapper.Should().NotBeNull();
        mapper.Should().BeOfType<TestContentMapper<TestModel>>();
    }

    [Test]
    public void CreateMapperForType_WhenServiceNotRegistered_ShouldThrow()
    {
        var provider = new ServiceCollection().BuildServiceProvider();
        var factory = new ContentMapperFactory(provider);

        Action act = () => factory.CreateMapperForType(typeof(TestModel));

        act.Should().Throw<InvalidOperationException>();
    }
}