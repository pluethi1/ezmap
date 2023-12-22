namespace EZMap.Mappers.Wrappers
{
    public class AsyncTypeMapperWrapper : ITypeMapper
    {
        private readonly IAsyncTypeMapper asyncTypeMapper;

        public AsyncTypeMapperWrapper(IAsyncTypeMapper asyncTypeMapper)
        {
            this.asyncTypeMapper = asyncTypeMapper;
        }

        public Type SourceType => asyncTypeMapper.SourceType;
        public Type TargetType => asyncTypeMapper.TargetType;

        public object Map(object source)
        {
            return asyncTypeMapper.MapAsync(source).GetAwaiter().GetResult();
        }
    }

    public class AsyncTypeMapperWrapper<TSource, TTarget> : AsyncTypeMapperWrapper, ITypeMapper<TSource, TTarget>
        where TSource : notnull
        where TTarget : notnull
    {
        public AsyncTypeMapperWrapper(IAsyncTypeMapper<TSource, TTarget> asyncTypeMapper)
            : base(asyncTypeMapper)
        {
        }

        public TTarget Map(TSource source)
        {
            return (TTarget)Map((object)source);
        }
    }
}