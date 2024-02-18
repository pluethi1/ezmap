namespace EZMap
{
    public class DictionaryTypeMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget>
        where TSource : notnull
        where TTarget : notnull
    {
        public TTarget Map(TSource source)
        {
            throw new NotImplementedException();
        }
    }
}