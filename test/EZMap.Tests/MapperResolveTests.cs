using EZMap.Builders;
using EZMap.Mappers;
using EZMap.Mappers.Wrappers;
using EZMap.Registration;
using EZMap.Tests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EZMap.Tests
{
    public class MapperResolveTests
    {
        [Fact]
        public void Should_ResolveAutoMapperCorrectly()
        {
            var services = TestUtilities.CreateDefaultServiceProvider();
            IMapper? mapper = null;
            Assert.Null(Record.Exception(() => mapper = services.GetRequiredService<IMapper>()));
            Assert.NotNull(mapper);
            Assert.IsType<AutoMapper>(mapper);
        }

        [Fact]
        public void Should_ResolveAutoTypeMapperCorrectly()
        {
            var services = TestUtilities.CreateDefaultServiceProvider();
            ITypeMapper<string, int>? mapper = null;
            Assert.Null(Record.Exception(() => mapper = services.GetRequiredService<ITypeMapper<string, int>>()));
            Assert.NotNull(mapper);
            Assert.IsType<ResolvingTypeMapper<string, int>>(mapper);
        }

        [Fact]
        public void Should_ResolveAsyncAutoTypeMapperCorrectly()
        {
            var services = TestUtilities.CreateDefaultServiceProvider();
            IAsyncTypeMapper<string, int>? mapper = null;
            Assert.Null(Record.Exception(() => mapper = services.GetRequiredService<IAsyncTypeMapper<string, int>>()));
            Assert.NotNull(mapper);
            Assert.IsType<ResolvingAsyncTypeMapper<string, int>>(mapper);
        }

        [Fact]
        public void Should_ResolveCustomTypeMapperCorrectly()
        {
            var services = TestUtilities.CreateDefaultServices(buildAction: builder =>
            {
                builder.RegisterTypeMapper<CustomTypeMapper, int, string>();
            });
            var provider = services.BuildServiceProvider();
            var resolver = provider.GetRequiredService<IMapperResolver>();
            ITypeMapper<int, string>? mapper = null;
            Assert.Null(Record.Exception(() => mapper = resolver.ResolveTypeMapper<int, string>()));
            Assert.NotNull(mapper);
            Assert.IsType<CustomTypeMapper>(mapper);
        }

        [Fact]
        public void Should_ResolveWrappedCustomAsyncTypeMapperCorrectly()
        {
            var services = TestUtilities.CreateDefaultServices(buildAction: builder =>
            {
                builder.RegisterTypeMapper<CustomTypeMapper, int, string>();
            });
            var provider = services.BuildServiceProvider();
            var resolver = provider.GetRequiredService<IMapperResolver>();
            IAsyncTypeMapper<int, string>? asyncMapper = null;
            Assert.Null(Record.Exception(() => asyncMapper = resolver.ResolveAsyncTypeMapper<int, string>()));
            Assert.NotNull(asyncMapper);
            Assert.IsType<SyncTypeMapperWrapper<int, string>>(asyncMapper);
        }
    }
}
