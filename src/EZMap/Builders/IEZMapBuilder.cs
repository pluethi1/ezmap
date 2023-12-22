namespace EZMap.Builders
{
    public interface IEZMapBuilder
    {
        IEZMapBuilder RegisterMapper(Type mapperType, Type sourceType, Type targetType);
        IEZMapBuilder UseCachedResolver(bool value = true);
        IEZMapBuilder ConfigureAutoMapper<TSource, TTarget>(Action<IAutoMapperBuilder<TSource, TTarget>> configure);
    }
}
