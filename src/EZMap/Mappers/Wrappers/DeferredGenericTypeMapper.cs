using EZMap.Utilities;

namespace EZMap.Mappers.Wrappers
{
    public class DeferredGenericTypeMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget>
        where TSource : notnull
        where TTarget : notnull
    {
        private readonly ITypeMapper parentTypeMapper;

        public DeferredGenericTypeMapper(ITypeMapper parentTypeMapper)
        {
            MapperUtilities.AssertEqualType<TSource>(parentTypeMapper.SourceType, nameof(parentTypeMapper));
            MapperUtilities.AssertEqualType<TTarget>(parentTypeMapper.TargetType, nameof(parentTypeMapper));
            this.parentTypeMapper = parentTypeMapper;
        }

        public Type SourceType => parentTypeMapper.SourceType;
        public Type TargetType => parentTypeMapper.TargetType;

        public TTarget Map(TSource source)
        {
            return (TTarget)parentTypeMapper.Map(source);
        }

        public object Map(object source)
        {
            return parentTypeMapper.Map(source);
        }
    }
}
