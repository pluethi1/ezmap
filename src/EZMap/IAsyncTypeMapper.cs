namespace EZMap
{
    public interface IAsyncTypeMapper
    {
        Type SourceType { get; }
        Type TargetType { get; }

        Task<object> MapAsync(object source, CancellationToken cancellationToken = default);
    }

    public interface IAsyncTypeMapper<TSource, TTarget> : IAsyncTypeMapper
        where TSource : notnull
        where TTarget : notnull
    {
        Type IAsyncTypeMapper.SourceType => typeof(TSource);
        Type IAsyncTypeMapper.TargetType => typeof(TTarget);
        async Task<object> IAsyncTypeMapper.MapAsync(object source, CancellationToken cancellationToken) => await MapAsync((TSource)source, cancellationToken);

        Task<TTarget> MapAsync(TSource source, CancellationToken cancellationToken = default);
    }
}