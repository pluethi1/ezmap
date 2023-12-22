namespace EZMap.Builders
{
    public interface ITargetMemberMapperBuilder<TSource, TTarget>
    {
        IAutoMapperBuilder<TSource, TTarget> Inject();
        IAutoMapperBuilder<TSource, TTarget> DefaultValue<TMember>(Func<TMember> valueProvider);
    }
}
