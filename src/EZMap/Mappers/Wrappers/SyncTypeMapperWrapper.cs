namespace EZMap.Mappers.Wrappers
{
    public class SyncTypeMapperWrapper : IAsyncTypeMapper
    {
        private readonly ITypeMapper typeMapper;

        public SyncTypeMapperWrapper(ITypeMapper typeMapper)
        {
            this.typeMapper = typeMapper;
        }

        public Type SourceType => typeMapper.SourceType;
        public Type TargetType => typeMapper.TargetType;

        public Task<object> MapAsync(object source, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<object>(cancellationToken);
            }
            return Task.FromResult(typeMapper.Map(source));
        }
    }

    public class SyncTypeMapperWrapper<TSource, TTarget> : SyncTypeMapperWrapper, IAsyncTypeMapper<TSource, TTarget>
        where TSource : notnull
        where TTarget : notnull
    {
        public SyncTypeMapperWrapper(ITypeMapper<TSource, TTarget> typeMapper)
            : base(typeMapper)
        {
        }

        public Task<TTarget> MapAsync(TSource source, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<TTarget>(cancellationToken);
            }
            return Task.FromResult((TTarget)MapAsync((object)source, cancellationToken).Result);
        }
    }
}
