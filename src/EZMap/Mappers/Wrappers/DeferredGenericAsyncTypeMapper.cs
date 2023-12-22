using EZMap.Utilities;

namespace EZMap.Mappers.Wrappers
{
    public class DeferredGenericAsyncTypeMapper<TSource, TTarget> : IAsyncTypeMapper<TSource, TTarget>
        where TSource : notnull
        where TTarget : notnull
    {
        private readonly IAsyncTypeMapper parentAsyncTypeMapper;

        public DeferredGenericAsyncTypeMapper(IAsyncTypeMapper parentAsyncTypeMapper)
        {
            MapperUtilities.AssertEqualType<TSource>(parentAsyncTypeMapper.SourceType, nameof(parentAsyncTypeMapper));
            MapperUtilities.AssertEqualType<TSource>(parentAsyncTypeMapper.TargetType, nameof(parentAsyncTypeMapper));
            this.parentAsyncTypeMapper = parentAsyncTypeMapper;
        }

        public Type SourceType => parentAsyncTypeMapper.SourceType;
        public Type TargetType => parentAsyncTypeMapper.TargetType;

        public async Task<TTarget> MapAsync(TSource source, CancellationToken cancellationToken = default)
        {
            return (TTarget)await parentAsyncTypeMapper.MapAsync(source, cancellationToken);
        }

        public Task<object> MapAsync(object source, CancellationToken cancellationToken = default)
        {
            return parentAsyncTypeMapper.MapAsync(source, cancellationToken);
        }
    }
}
