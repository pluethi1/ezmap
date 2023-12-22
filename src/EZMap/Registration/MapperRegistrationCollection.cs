using System.Diagnostics.CodeAnalysis;
using EZMap.Utilities;

namespace EZMap.Registration
{
    internal class MapperRegistrationCollection : IEnumerable<Type>
    {
        private readonly Dictionary<MapperKey, Type> registrations = new();

        internal void Add(Type sourceType, Type targetType, Type mapperType)
        {
            var key = new MapperKey(sourceType, targetType);
            Add(key, mapperType);
        }

        internal void Add(MapperKey key, Type mapperType)
        {
            registrations.Add(key, mapperType);
        }

        internal Type this[MapperKey key]
        {
            get => registrations[key];
            set => registrations[key] = AssertValidMapperType(value, nameof(value));
        }

        internal bool TryGetMapperType(MapperKey key, [NotNullWhen(true)]out Type? result)
        {
            return registrations.TryGetValue(key, out result);
        }

        public IEnumerator<Type> GetEnumerator() => registrations.Values.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => registrations.Values.GetEnumerator();

        private static Type AssertValidMapperType(Type type, string parameterName)
        {
            if (!type.IsAssignableToAny(typeof(IMapper), typeof(IAsyncMapper), typeof(ITypeMapper), typeof(IAsyncTypeMapper)))
            {
                throw new ArgumentException("Invalid mapper type", parameterName);
            }
            return type;
        }
    }
}
