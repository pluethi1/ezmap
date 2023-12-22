using EZMap.Utilities;

namespace EZMap.Mappers.Wrappers
{
    public class DeferredAsyncMapper : IAsyncMapper
    {
        private readonly IAsyncTypeMapper asyncTypeMapper;

        public DeferredAsyncMapper(IAsyncTypeMapper asyncTypeMapper)
        {
            this.asyncTypeMapper = asyncTypeMapper;
        }

        public Task<object> MapAsync(object source, Type sourceType, Type targetType, CancellationToken cancellationToken = default)
        {
            MapperUtilities.AssertEqualType(sourceType, asyncTypeMapper.SourceType, nameof(sourceType));
            MapperUtilities.AssertEqualType(targetType, asyncTypeMapper.TargetType, nameof(targetType));
            return asyncTypeMapper.MapAsync(source, cancellationToken);
        }
    }
}