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
        TTarget Map(TSource source);
    }
}