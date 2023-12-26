using EZMap.Configuration;

namespace EZMap.Registration
{
    internal class MapperRegistration
    {
        internal MapperRegistration(Type mapperType)
        {
            MapperType = mapperType;
        }

        internal MapperRegistration(AutoMapperConfiguration autoMapperConfiguration)
        {
            AutoMapperConfiguration = autoMapperConfiguration;
        }

        internal Type? MapperType { get; }
        internal AutoMapperConfiguration? AutoMapperConfiguration { get; }
    }
}
