using EZMap.Configuration;

namespace EZMap.Registration.Activation
{
    internal class MapperActivationContext
    {
        internal MapperActivationContext(Type requestedType,
                                         Type? registeredType,
                                         Type sourceType,
                                         Type targetType,
                                         MapperSettings mapperSettings,
                                         IMapperResolver mapperResolver,
                                         IServiceProvider serviceProvider)
        {
            RequestedType = requestedType;
            RegisteredType = registeredType;
            SourceType = sourceType;
            TargetType = targetType;
            MapperSettings = mapperSettings;
            MapperResolver = mapperResolver;
            ServiceProvider = serviceProvider;
        }

        internal Type RequestedType { get; }
        internal Type? RegisteredType { get; }
        internal Type SourceType { get; }
        internal Type TargetType { get; }
        internal MapperSettings MapperSettings { get; }
        internal IMapperResolver MapperResolver { get; }
        internal IServiceProvider ServiceProvider { get; }

    }
}