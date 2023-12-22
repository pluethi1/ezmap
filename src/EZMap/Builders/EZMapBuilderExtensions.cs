namespace EZMap.Builders
{
    public static class EZMapBuilderExtensions
    {
        public static IEZMapBuilder RegisterMapper<TMapper, TSource, TTarget>(this IEZMapBuilder builder)
            where TMapper : IMapper
            where TSource : notnull
            where TTarget : notnull
            => builder.RegisterMapper(typeof(TMapper), typeof(TSource), typeof(TTarget));

        public static IEZMapBuilder RegisterTypeMapper<TMapper, TSource, TTarget>(this IEZMapBuilder builder)
            where TMapper : ITypeMapper<TSource, TTarget>
            where TSource : notnull
            where TTarget : notnull
            => builder.RegisterMapper(typeof(TMapper), typeof(TSource), typeof(TTarget));

        public static IEZMapBuilder RegisterAsyncMapper<TMapper, TSource, TTarget>(this IEZMapBuilder builder)
            where TMapper : IAsyncMapper
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
