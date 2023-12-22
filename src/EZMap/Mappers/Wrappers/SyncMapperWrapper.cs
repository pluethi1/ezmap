namespace EZMap.Mappers.Wrappers
{
    public class SyncMapperWrapper : IAsyncMapper
    {
        private readonly IMapper mapper;

        public SyncMapperWrapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Task<object> MapAsync(object source, Type sourceType, Type targetType, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<object>(cancellationToken);
            }

            return Task.FromResult(mapper.Map(source, sourceType, targetType));
        }
    }
}