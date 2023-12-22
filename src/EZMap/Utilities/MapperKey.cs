using System.Diagnostics.CodeAnalysis;

namespace EZMap.Utilities
{
    internal readonly struct MapperKey : IEquatable<MapperKey>
    {
        internal MapperKey(Type sourceType, Type targetType)
        {
            SourceType = sourceType;
            TargetType = targetType;
        }

        internal Type SourceType { get; }
        internal Type TargetType { get; }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is MapperKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SourceType.GetHashCode(), TargetType.GetHashCode());
        }

        public bool Equals(MapperKey other)
        {
            return SourceType.Equals(other.SourceType) &&
                   TargetType.Equals(other.TargetType);
        }
    }
}