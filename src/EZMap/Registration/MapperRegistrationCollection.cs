using System.Diagnostics.CodeAnalysis;
using EZMap.Configuration;
using EZMap.Utilities;

namespace EZMap.Registration
{
    internal class MapperRegistrationCollection : IEnumerable<KeyValuePair<MapperKey, MapperRegistration>>
    {
        private readonly Dictionary<MapperKey, MapperRegistration> registrations = new();

        internal void Add(Type sourceType, Type targetType, Type mapperType)
        {
            var key = new MapperKey(sourceType, targetType);
            Add(key, mapperType);
        }

        internal void Add(MapperKey key, Type mapperType)
        {
            registrations.Add(key, new MapperRegistration(AssertValidMapperType(key, mapperType, nameof(mapperType))));
        }

        internal void Add(Type sourceType, Type targetType, AutoMapperConfiguration autoMapperConfiguration)
        {
            var key = new MapperKey(sourceType, targetType);
            Add(key, autoMapperConfiguration);
        }

        internal void Add(MapperKey key, AutoMapperConfiguration autoMapperConfiguration)
        {
            registrations.Add(key, new MapperRegistration(autoMapperConfiguration));
        }

        internal MapperRegistration this[MapperKey key]
        {
            get => registrations[key];
            set => registrations[key] = value;
        }

        internal bool TryGetMapperType(MapperKey key, [NotNullWhen(true)]out Type? type)
        {
            if (registrations.TryGetValue(key, out var registration))
            {
                type = registration.MapperType;
                return type != null;
            }
            type = null;
            return false;
        }

        internal bool TryGetAutoMapperConfiguration(MapperKey key, [NotNullWhen(true)]out AutoMapperConfiguration? autoMapperConfiguration)
        {
            if (registrations.TryGetValue(key, out var registration))
            {
                autoMapperConfiguration = registration.AutoMapperConfiguration;
                return autoMapperConfiguration != null;
            }
            autoMapperConfiguration = null;
            return false;
        }

        public IEnumerator<KeyValuePair<MapperKey, MapperRegistration>> GetEnumerator() => registrations.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => registrations.Values.GetEnumerator();

        private static Type AssertValidMapperType(in MapperKey key, Type type, string parameterName)
        {
            foreach (var interfaceType in type.GetInterfaces())
            {
                if (!interfaceType.IsGenericType)
                {
                    continue;
                }

                var typeDefinition = interfaceType.GetGenericTypeDefinition();

                if (typeDefinition != typeof(ITypeMapper<,>) &&
                    typeDefinition != typeof(IAsyncTypeMapper<,>))
                {
                    continue;
                }

                var genericArguments = interfaceType.GetGenericArguments();

                if (genericArguments[0] != key.SourceType &&
                    genericArguments[1] != key.TargetType)
                {
                    continue;
                }

                return type;
            }
            throw new ArgumentException("Invalid mapper type", parameterName);
        }
    }
}