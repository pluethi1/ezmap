namespace EZMap.Mappers.Wrappers
{
    public class DeferredTypeMapper : ITypeMapper
    {
        private readonly IMapper parentMapper;

        public DeferredTypeMapper(Type sourceType, Type targetType, IMapper parentMapper)
        {
            this.parentMapper = parentMapper;
            SourceType = sourceType;
            TargetType = targetType;
        }

        public Type SourceType { get; }
        public Type TargetType { get; }

        public object Map(object source)
        {
            return parentMapper.Map(source, SourceType, TargetType);
        }
    }

    public class DeferredTypeMapper<TSource, TTarget> : DeferredTypeMapper, ITypeMapper<TSource, TTarget>
        where TSource : notnull
        where TTarget : notnull
    {
        public DeferredTypeMapper(IMapper parentMapper)
            : base(typeof(TSource), typeof(TTarget), parentMapper)
        {
        }

        public TTarget Map(TSource source)
        {
            return (TTarget)Map((object)source);
        }
    }
}