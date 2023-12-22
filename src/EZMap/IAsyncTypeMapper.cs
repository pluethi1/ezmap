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
        Task<TTarget> MapAsync(TSource source, CancellationToken cancellationToken = default);
    }
}