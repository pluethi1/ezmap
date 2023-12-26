namespace EZMap
{
    public interface IMapper
    {
        object Map(object source, Type sourceType, Type targetType);
        Task<object> MapAsync(object source, Type sourceType, Type targetType, CancellationToken cancellationToken = default);
    }
}