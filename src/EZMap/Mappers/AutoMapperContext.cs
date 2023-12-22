using EZMap.Configuration;
using EZMap.Registration;
using EZMap.Utilities;
using System.Reflection;

namespace EZMap.Mappers
{
    internal class AutoMapperContext
    {
        internal static AutoMapperContext Create(Type sourceType,
                                                 Type targetType,
                                                 MapperSettings settings,
                                                 IMapperResolver resolver,
                                                 IServiceProvider serviceProvider)
        {

        }

        internal AutoMapperContext(Dictionary<string, MemberInfo> sourceMembers,
                                   Dictionary<string, MemberInfo> targetMembers,
                                   Dictionary<string, MappableMember> autoMappableMembers,
                                   object source,
                                   object target,
                                   IMapperResolver mapperResolver,
                                   IServiceProvider serviceProvider)
        {
            SourceMembers = sourceMembers;
            TargetMembers = targetMembers;
            AutoMappableMembers = autoMappableMembers;
            Source = source;
            Target = target;
            MapperResolver = mapperResolver;
            ServiceProvider = serviceProvider;
        }

        internal Dictionary<string, MemberInfo> SourceMembers { get; }
        internal Dictionary<string, MemberInfo> TargetMembers { get; }
        internal Dictionary<string, MappableMember> AutoMappableMembers { get; }
        
        internal object Source { get; }
        internal object Target { get; }
        internal IMapperResolver MapperResolver { get; }
        internal IServiceProvider ServiceProvider { get; }
    }
}