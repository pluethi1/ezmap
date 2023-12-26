using EZMap.Configuration;
using EZMap.Utilities;

namespace EZMap.Builders
{
    public class TargetMemberAutoMapperBuilder<TSource, TTarget, TMember> : ITargetMemberMapperBuilder<TSource, TTarget, TMember>
    {
        private readonly string memberName;
        private readonly AutoMapperBuilder<TSource, TTarget> autoMapperBuilder;

        public TargetMemberAutoMapperBuilder(string memberName, AutoMapperBuilder<TSource, TTarget> autoMapperBuilder)
        {
            this.autoMapperBuilder = autoMapperBuilder;
            this.memberName = memberName;
        }

        public IAutoMapperBuilder<TSource, TTarget> DefaultValue(Func<TMember> valueProvider)
        {
            //ToDo: find a way to avoid wrapping the valueProvider delegate (can't be cast because of boxing shenanigans)
            autoMapperBuilder.targetMemberConfiguration.Add(memberName, new AutoMapperTargetMemberConfiguration(false, null, () => valueProvider()!, null, null));
            return autoMapperBuilder;
        }

        public IAutoMapperBuilder<TSource, TTarget> Inject()
        {
            autoMapperBuilder.targetMemberConfiguration.Add(memberName, new AutoMapperTargetMemberConfiguration(true, null, null, null, null));
            return autoMapperBuilder;
        }

        public IAutoMapperBuilder<TSource, TTarget> Enrich(Func<TSource, TTarget, TMember> enrichFunction)
        {
            object WrappedEnrichFunction(object source, object target) => enrichFunction((TSource)source, (TTarget)target)!;
            autoMapperBuilder.targetMemberConfiguration.Add(memberName, new AutoMapperTargetMemberConfiguration(false, null, null, WrappedEnrichFunction, null));
            return autoMapperBuilder;
        }

        public IAutoMapperBuilder<TSource, TTarget> EnrichAsync(Func<TSource, TTarget, CancellationToken, Task<TMember>> asyncEnrichFunction)
        {
            async Task<object> WrappedAsyncEnrichFunction(object source, object target, CancellationToken cancellationToken) =>
                (await asyncEnrichFunction((TSource)source, (TTarget)target, cancellationToken))!;
            autoMapperBuilder.targetMemberConfiguration.Add(memberName, new AutoMapperTargetMemberConfiguration(false, null, null, null, WrappedAsyncEnrichFunction));
            return autoMapperBuilder;
        }
    }
}
