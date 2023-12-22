using EZMap.Configuration;
using EZMap.Registration;
using EZMap.Utilities;

namespace EZMap.Mappers
{
    public class AutoMapper : IMapper
    {
        private readonly MapperSettings mapperSettings;
        private readonly IMapperResolver mapperResolver;
        private readonly IServiceProvider serviceProvider;

        public AutoMapper(MapperSettings mapperSettings, IMapperResolver mapperResolver, IServiceProvider serviceProvider)
        {
            this.mapperSettings = mapperSettings;
            this.mapperResolver = mapperResolver;
            this.serviceProvider = serviceProvider;
        }

        public object Map(object source, Type sourceType, Type targetType)
        {
            var mappableMembers = MapperUtilities.GetMatchingFieldsAndProperties(sourceType, targetType);

            var target = MapperUtilities.CreateObject(targetType, serviceProvider);
            
            foreach (var member in mappableMembers)
            {
                try
                {
                    var memberSourceType = member.SourceMember.GetMemberType();
                    var memberTargetType = member.TargetMember.GetMemberType();

                    var memberMapper = mapperResolver.ResolveTypeMapper(memberSourceType, memberTargetType);
                    var sourceValue = member.SourceMember.GetMemberValue(source);
                    var targetValue = memberMapper.Map(source);
                    member.TargetMember.SetMemberValue(target, targetValue);
                }
                catch
                {
                    if (mapperSettings.AutoMapperExceptionBehavior == AutoMapperExceptionBehavior.Propagate)
                    {
                        throw;
                    }
                }
            }

            return target;
        }
    }
}