using System.Linq.Expressions;

namespace EZMap.Builders
{
    public interface IAutoMapperBuilder<TSource, TTarget>
    {
        ISourceMemberMapperBuilder<TSource, TTarget> Source<TMember>(Expression<Func<TSource, TMember>> expression);
        ITargetMemberMapperBuilder<TSource, TTarget, TMember> Target<TMember>(Expression<Func<TTarget, TMember>> expression);
    }
}
