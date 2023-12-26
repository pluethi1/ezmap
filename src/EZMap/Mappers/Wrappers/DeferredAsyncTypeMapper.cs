namespace EZMap.Mappers.Wrappers
{
    public class DeferredAsyncTypeMapper<TSource, TTarget> : IAsyncTypeMapper<TSource, TTarget>
        where TSource : notnull
        where TTarget : notnull
    {
        private readonly IMapper mapper;

        public DeferredAsyncTypeMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<TTarget> MapAsync(TSource source, CancellationToken cancellationToken = default)
        {
            return (TTarget)await mapper.MapAsync(source, typeof(TSource), typeof(TTarget), cancellationToken);
        }
    }
}
