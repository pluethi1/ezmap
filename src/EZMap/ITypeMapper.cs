namespace EZMap
{
    public interface ITypeMapper
    {
        Type SourceType { get; }
        Type TargetType { get; }

        object Map(object source);
    }

    public interface ITypeMapper<TSource, TTarget> : ITypeMapper
        where TSource : notnull
        where TTarget : notnull
    {
        Type ITypeMapper.SourceType => typeof(TSource);
        Type ITypeMapper.TargetType => typeof(TTarget);
        object ITypeMapper.Map(object source) => Map((TSource)source);
        TTarget Map(TSource source);
    }
}