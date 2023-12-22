namespace EZMap.Mappers.Wrappers
{
    public class AsyncMapperWrapper : IMapper
    {
        private readonly IAsyncMapper asyncMapper;

        public AsyncMapperWrapper(IAsyncMapper asyncMapper)
        {
            this.asyncMapper = asyncMapper;
        }

        public object Map(object source, Type sourceType, Type targetType)
        {
            return asyncMapper.MapAsync(source, sourceType, targetType).GetAwaiter().GetResult();
        }
    }
}
