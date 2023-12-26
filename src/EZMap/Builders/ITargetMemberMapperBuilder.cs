namespace EZMap.Builders
{
    public interface ITargetMemberMapperBuilder<TSource, TTarget, TMember>
    {
        IAutoMapperBuilder<TSource, TTarget> Inject();
        IAutoMapperBuilder<TSource, TTarget> DefaultValue(Func<TMember> valueProvider);
        IAutoMapperBuilder<TSource, TTarget> Enrich(Func<TSource, TTarget, TMember> enrichFunction);
        IAutoMapperBuilder<TSource, TTarget> EnrichAsync(Func<TSource, TTarget, CancellationToken, Task<TMember>> asyncEnrichFunction);
    }
}
