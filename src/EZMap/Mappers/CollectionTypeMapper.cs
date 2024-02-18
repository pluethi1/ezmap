using EZMap.Utilities;

namespace EZMap.Mappers
{
    public class CollectionTypeMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget>
        where TSource : notnull
        where TTarget : notnull
    {
        public CollectionTypeMapper()
        {
            MapperUtilities.AssertReadableCollectionType<TSource>(nameof(TSource));
            MapperUtilities.AssertWritableCollectionType<TTarget>(nameof(TTarget));
        }

        public TTarget Map(TSource source)
        {
            throw new NotImplementedException();
        }
    }
}
