using EZMap.Utilities;

namespace EZMap.Mappers.Wrappers
{
    public class DeferredMapper : IMapper
    {
        private readonly ITypeMapper typeMapper;

        public DeferredMapper(ITypeMapper typeMapper)
        {
            this.typeMapper = typeMapper;
        }

        public object Map(object source, Type sourceType, Type targetType)
        {
            MapperUtilities.AssertEqualType(sourceType, typeMapper.SourceType, nameof(sourceType));
            MapperUtilities.AssertEqualType(targetType, typeMapper.TargetType, nameof(targetType));
            return typeMapper.Map(source);
        }

        public Task<object> MapAsync(object source, Type sourceType, Type targetType, CancellationToken cancellationToken = default) 
            => MapperUtilities.MakeTask(() => Map(source, sourceType, targetType), cancellationToken);
    }
}