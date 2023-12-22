namespace EZMap.Registration
{
    public interface IMapperResolver
    {
        object Resolve(Type requestedMapperType, Type sourceType, Type targetType);
    }
}
