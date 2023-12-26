using EZMap.Configuration;
using EZMap.Utilities;

namespace EZMap.Builders
{
    public class AutoMapperBuilder<TSource, TTarget> : IAutoMapperBuilder<TSource, TTarget>
    {
        internal readonly AutoMapperMemberConfigurationCollection<AutoMapperSourceMemberConfiguration> sourceMemberConfiguration = new(typeof(TSource));
        internal readonly AutoMapperMemberConfigurationCollection<AutoMapperTargetMemberConfiguration> targetMemberConfiguration = new(typeof(TTarget));

        public ISourceMemberMapperBuilder<TSource, TTarget> Source<TMember>(System.Linq.Expressions.Expression<Func<TSource, TMember>> expression)
        {
            var sourceMemberName = MapperUtilities.AssertValidMemberAndGetName(expression, true, nameof(expression));
            return new SourceMemberAutoMapperBuilder<TSource, TTarget>(sourceMemberName, this);
        }

        public ITargetMemberMapperBuilder<TSource, TTarget, TMember> Target<TMember>(System.Linq.Expressions.Expression<Func<TTarget, TMember>> expression)
        {
            var targetMemberName = MapperUtilities.AssertValidMemberAndGetName(expression, false, nameof(expression));
            return new TargetMemberAutoMapperBuilder<TSource, TTarget, TMember>(targetMemberName, this);
        }

        internal AutoMapperConfiguration Build()
        {
            return new AutoMapperConfiguration(sourceMemberConfiguration, targetMemberConfiguration);
        }
    }
}
