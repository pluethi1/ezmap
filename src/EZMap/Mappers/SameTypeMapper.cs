using System.Runtime.CompilerServices;

namespace EZMap.Mappers
{
    public class SameTypeMapper<T> : ITypeMapper<T, T>
        where T : notnull
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Map(T source)
        {
            return source;
        }
    }
}
