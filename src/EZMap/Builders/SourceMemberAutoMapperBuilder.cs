using EZMap.Configuration;
using EZMap.Utilities;
using System.Linq.Expressions;

namespace EZMap.Builders
{
    public class SourceMemberAutoMapperBuilder<TSource, TTarget> : ISourceMemberMapperBuilder<TSource, TTarget>
    {
        private readonly string memberName;
        private readonly AutoMapperBuilder<TSource, TTarget> autoMapperBuilder;

        public SourceMemberAutoMapperBuilder(string memberName, AutoMapperBuilder<TSource, TTarget> autoMapperBuilder)
        {
            this.memberName = memberName;
            this.autoMapperBuilder = autoMapperBuilder;
        }

        public IAutoMapperBuilder<TSource, TTarget> Ignore()
        {
            autoMapperBuilder.sourceMemberConfiguration.Add(memberName, new AutoMapperSourceMemberConfiguration(true, null));
            return autoMapperBuilder;

        }

        public IAutoMapperBuilder<TSource, TTarget> MapTo<TMember>(Expression<Func<TTarget, TMember>> expression)
        {
            var targetMemberName = MapperUtilities.AssertValidMemberAndGetName(expression, true, nameof(expression));
            autoMapperBuilder.sourceMemberConfiguration.Add(memberName, new AutoMapperSourceMemberConfiguration(false, targetMemberName));
            return autoMapperBuilder;
        }
    }
}
