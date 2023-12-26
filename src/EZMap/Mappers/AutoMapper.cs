using EZMap.Configuration;
using EZMap.Registration;
using EZMap.Utilities;
using System.Reflection;

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
            return MapInternal(source, sourceType, targetType, AutoMap).Result;
        }

        public async Task<object> MapAsync(object source, Type sourceType, Type targetType, CancellationToken cancellationToken = default) 
        {
            return await MapInternal(source, sourceType, targetType, AutoMapAsync, cancellationToken);
        }

        private async Task<object> MapInternal(object source, Type sourceType, Type targetType, AutoMapDelegate autoMapperFunction, CancellationToken cancellationToken = default)
        {
            var mapperKey = new MapperKey(sourceType, targetType);
            if (!mapperSettings.Registrations.TryGetAutoMapperConfiguration(mapperKey, out var autoMapperConfiguration))
            {
                return await MapInternalFast(source, sourceType, targetType, autoMapperFunction, cancellationToken);
            }

            var target = MapperUtilities.CreateObject(targetType, serviceProvider);

            var sourceMembers = MapperUtilities.GetReadableFieldsAndProperties(sourceType);
            var targetMembers = MapperUtilities.GetWriteableFieldsAndProperties(targetType);

            foreach (var targetMember in targetMembers)
            {
                await HandleTargetMember(autoMapperConfiguration, autoMapperFunction, source, target, targetMember, sourceMembers, cancellationToken);
            }

            return target;
        }

        private async Task HandleTargetMember(AutoMapperConfiguration autoMapperConfiguration,
                                              AutoMapDelegate autoMapperFunction,
                                              object source,
                                              object target,
                                              MemberInfo targetMember,
                                              IEnumerable<MemberInfo> sourceMembers,
                                              CancellationToken cancellationToken)
        {
            var targetType = targetMember.GetMemberType();
            MemberInfo? sourceMember = null;
            object? targetValue = null;
            if (autoMapperConfiguration.TargetMemberConfigurations.TryGetMemberConfiguration(targetMember.Name, out var targetMemberConfiguration))
            {
                if (targetMemberConfiguration.Inject)
                {
                    targetValue = serviceProvider.GetService(targetType);
                    if (targetValue == null && mapperSettings.AutoMapperExceptionBehavior == AutoMapperExceptionBehavior.Propagate)
                    {
                        throw new InvalidOperationException($"Could not inject member {targetMember.Name} of type {targetType.Name}");
                    }
                }
                else if (targetMemberConfiguration.DefaultValueFactory != null)
                {
                    targetValue = targetMemberConfiguration.DefaultValueFactory();
                }
                else if (targetMemberConfiguration.EnrichFunction != null)
                {
                    targetValue = targetMemberConfiguration.EnrichFunction(source, target);
                }
                else if (targetMemberConfiguration.AsyncEnrichFunction != null)
                {
                    targetValue = await targetMemberConfiguration.AsyncEnrichFunction(source, target, cancellationToken);
                }
                else
                {
                    if (targetMemberConfiguration.SourceMemberName != null)
                    {
                        sourceMember = sourceMembers.Single(x => x.Name == targetMemberConfiguration.SourceMemberName);
                    }
                    else
                    {
                        sourceMember = sourceMembers.SingleOrDefault(x => x.Name == targetMember.Name);
                    }

                    if (sourceMember == null)
                    {
                        return;
                    }

                    if (autoMapperConfiguration.SourceMemberConfigurations.TryGetMemberConfiguration(sourceMember.Name, out var sourceMemberConfiguration) && sourceMemberConfiguration.Ignore)
                    {
                        return;
                    }
                }

                if (sourceMember != null)
                {
                    await autoMapperFunction(source, target, sourceMember, targetMember, cancellationToken);
                }
                else
                {
                    targetMember.SetMemberValue(target, targetValue);
                }
            }
        }

        private async Task<object> MapInternalFast(object source, Type sourceType, Type targetType, AutoMapDelegate autoMapperFunction, CancellationToken cancellationToken = default)
        {
            var mappableMembers = MapperUtilities.GetMatchingFieldsAndProperties(sourceType, targetType);
            var target = MapperUtilities.CreateObject(targetType, serviceProvider);

            foreach (var mappableMember in mappableMembers) 
            {
                await autoMapperFunction(source, target, mappableMember.SourceMember, mappableMember.TargetMember, cancellationToken);
            }
            return target;
        }

        private Task AutoMap(object source, object target, MemberInfo sourceMember, MemberInfo targetMember, CancellationToken cancellationToken)
        {
            var typeMapper = mapperResolver.ResolveTypeMapper(sourceMember.GetMemberType(), targetMember.GetMemberType());
            var sourceValue = sourceMember.GetMemberValue(source);
            object? targetValue = null;
            if (sourceValue != null)
            {
                targetValue = typeMapper.Map(sourceValue);
            }
            targetMember.SetMemberValue(target, targetValue);
            return Task.CompletedTask;
        }

        private async Task AutoMapAsync(object source, object target, MemberInfo sourceMember, MemberInfo targetMember, CancellationToken cancellationToken)
        {
            var asyncTypeMapper = mapperResolver.ResolveAsyncTypeMapper(sourceMember.GetMemberType(), targetMember.GetMemberType());
            var sourceValue = sourceMember.GetMemberValue(source);
            object? targetValue = null;
            if (sourceValue != null)
            {
                targetValue = await asyncTypeMapper.MapAsync(sourceValue, cancellationToken).ConfigureAwait(false);
            }
            targetMember.SetMemberValue(target, targetValue);
        }

        private delegate Task AutoMapDelegate(object source, object target, MemberInfo sourceMember, MemberInfo targetMember, CancellationToken cancellationToken);
    }
}