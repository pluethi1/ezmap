using EZMap.Configuration;
using EZMap.Mappers;
using EZMap.Registration.Activation;
using EZMap.Utilities;

namespace EZMap.Registration
{
    internal class DefaultMapperResolver : IMapperResolver
    {
        private readonly MapperSettings mapperSettings;
        private readonly IServiceProvider serviceProvider;

        public DefaultMapperResolver(MapperSettings mapperSettings, IServiceProvider serviceProvider)
        {
            this.mapperSettings = mapperSettings;
            this.serviceProvider = serviceProvider;
        }

        public object Resolve(Type requestedMapperType, Type sourceType, Type targetType)
        {
            var registrationKey = new MapperKey(sourceType, targetType);

            if (!mapperSettings.Registrations.TryGetMapperType(registrationKey, out var registeredType))
            {
                if (sourceType == targetType)
                {
                    registeredType = typeof(SameTypeMapper<>).MakeGenericType(sourceType);
                } else if (MapperUtilities.IsPrimitiveType(sourceType) && 
                           MapperUtilities.IsPrimitiveType(targetType))
                {
                    registeredType = typeof(PrimitiveTypeMapper<,>).MakeGenericType(sourceType, targetType);
                }
                
            }

            var activationContext = new MapperActivationContext(requestedMapperType,
                                                                registeredType,
                                                                sourceType,
                                                                targetType,
                                                                mapperSettings,
                                                                this,
                                                                serviceProvider);

            return MapperActivator.Create(activationContext);
        }
    }
}
