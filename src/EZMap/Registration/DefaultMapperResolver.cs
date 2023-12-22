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
        private readonly MapperRegistrationCollection registrations;

        internal DefaultMapperResolver(MapperSettings mapperSettings, IServiceProvider serviceProvider, MapperRegistrationCollection registrations)
        {
            this.mapperSettings = mapperSettings;
            this.serviceProvider = serviceProvider;
            this.registrations = registrations;
        }

        public object Resolve(Type requestedMapperType, Type sourceType, Type targetType)
        {
            var registrationKey = new MapperKey(sourceType, targetType);

            // Special case if both types are primitive types and there is no custom mapper registered
            if (!registrations.TryGetMapperType(registrationKey, out var registeredType) &&
                MapperUtilities.IsPrimitiveType(sourceType) &&
                MapperUtilities.IsPrimitiveType(targetType))
            {
                registeredType = typeof(PrimitiveMapper);
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
