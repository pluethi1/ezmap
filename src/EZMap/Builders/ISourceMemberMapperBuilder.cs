using System.Linq.Expressions;

namespace EZMap.Builders
{
    public interface ISourceMemberMapperBuilder<TSource, TTarget>
    {
        IAutoMapperBuilder<TSource, TTarget> MapTo<TMember>(Expression<Func<TTarget, TMember>> expression);
        IAutoMapperBuilder<TSource, TTarget> Ignore();
    }
}
