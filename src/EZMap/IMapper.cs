namespace EZMap
{
    public interface IMapper
    {
        object Map(object source, Type sourceType, Type targetType);
    }
}