namespace EZMap.Mappers.Wrappers
{
    public class DeferredAsyncTypeMapper : IAsyncTypeMapper
    {
        private readonly IAsyncMapper parentMapper;

        public DeferredAsyncTypeMapper(Type sourceType, Type targetType, IAsyncMapper parentMapper)
        {
            this.parentMapper = parentMapper;
            SourceType = sourceType;
            TargetType = targetType;
        }

        public Type SourceType { get; }
        public Type TargetType { get; }

        public Task<object> MapAsync(object source, CancellationToken cancellationToken = default)
        {
            return parentMapper.MapAsync(source, SourceType, TargetType, cancellationToken);
        }
    }

    public class DeferredAsyncTypeMapper<TSource, TTarget> : DeferredAsyncTypeMapper, IAsyncTypeMapper<TSource, TTarget>
        where TSource : notnull
        where TTarget : notnull
    {
        public DeferredAsyncTypeMapper(IAsyncMapper parentMapper)
            : base(typeof(TSource), typeof(TTarget), parentMapper)
        {
        }

        public async Task<TTarget> MapAsync(TSource source, CancellationToken cancellationToken = default)
        {
            return (TTarget)await MapAsync((object)source, cancellationToken);
        }
    }
}