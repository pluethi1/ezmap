namespace EZMap
{
    public interface IAsyncMapper
    {
        Task<object> MapAsync(object source, Type sourceType, Type targetType, CancellationToken cancellationToken = default);
    }
}
