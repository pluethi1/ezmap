namespace EZMap.Registration
{
    public static class MapperResolverExtensions
    {
        public static ITypeMapper ResolveTypeMapper(this IMapperResolver resolver, Type sourceType, Type targetType)
        {
            return Resolve<ITypeMapper>(resolver, sourceType, targetType);
        }

        public static ITypeMapper<TSource, TTarget> ResolveTypeMapper<TSource, TTarget>(this IMapperResolver resolver)
            where TSource : notnull
            where TTarget : notnull
        {
            return Resolve<ITypeMapper<TSource, TTarget>>(resolver, typeof(TSource), typeof(TTarget));
        }

        public static IAsyncTypeMapper ResolveAsyncTypeMapper(this IMapperResolver resolver, Type sourceType, Type targetType)
        {
            return Resolve<IAsyncTypeMapper>(resolver, sourceType, targetType);
        }

        public static IAsyncTypeMapper<TSource, TTarget> ResolveAsyncTypeMapper<TSource, TTarget>(this IMapperResolver resolver)
            where TSource : notnull
            where TTarget : notnull
        {
            return Resolve<IAsyncTypeMapper<TSource, TTarget>>(resolver, typeof(TSource), typeof(TTarget));
        }

        public static TMapper Resolve<TMapper>(this IMapperResolver resolver, Type sourceType, Type targetType)
        {
            return (TMapper)resolver.Resolve(typeof(TMapper), sourceType, targetType);
        }
    }
}
