namespace EZMap
{
    public static class MapperExtensions
    {
        public static TTarget Map<TSource, TTarget>(this IMapper mapper, TSource source)
            where TSource : notnull
            where TTarget : notnull
        {
            return (TTarget)mapper.Map(source, typeof(TSource), typeof(TTarget));
        }
    }
}
