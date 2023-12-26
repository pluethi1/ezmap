namespace EZMap.Builders
{
    public static class EZMapBuilderExtensions
    {
        public static IEZMapBuilder RegisterTypeMapper<TMapper, TSource, TTarget>(this IEZMapBuilder builder)
            where TMapper : ITypeMapper<TSource, TTarget>
            where TSource : notnull
            where TTarget : notnull
            => builder.RegisterMapper(typeof(TMapper), typeof(TSource), typeof(TTarget));

        public static IEZMapBuilder RegisterAsyncTypeMapper<TMapper, TSource, TTarget>(this IEZMapBuilder builder)
            where TMapper : IAsyncTypeMapper<TSource, TTarget>
            where TSource : notnull
            where TTarget : notnull
            => builder.RegisterMapper(typeof(TMapper), typeof(TSource), typeof(TTarget));
            
    }
}
