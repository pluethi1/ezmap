using EZMap.Configuration;
using EZMap.Mappers;
using EZMap.Registration;
using EZMap.Tests.Models;

namespace EZMap.Tests
{
    public class AutoMapperTests
    {
        [Fact]
        public void Should_AutoMapCorrectly()
        {
            var services = TestUtilities.CreateDefaultServiceProvider();
            var autoMapper = new AutoMapper(MapperSettings.Default, new DefaultMapperResolver(MapperSettings.Default, services), services);

            var fixture = new SampleSourceType
            {
                Id = 1,
                Age = 12,
            };

            SampleTargetType? target = null;
            Assert.Null(Record.Exception(() => target = autoMapper.Map<SampleSourceType, SampleTargetType>(fixture)));
            Assert.NotNull(target);
            Assert.Equal(fixture.Id, target.Id);
            Assert.Equal(fixture.Name, target.Name);
            Assert.Equal(fixture.Description, target.Description);
            Assert.NotEqual(fixture.Age, target.Age);
        }
    }
}
