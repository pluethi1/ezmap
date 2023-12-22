using EZMap.Configuration;

namespace EZMap.Builders
{
    public class TargetMemberAutoMapperBuilder<TSource, TTarget> : ITargetMemberMapperBuilder<TSource, TTarget>
    {
        private readonly string memberName;
        private readonly AutoMapperBuilder<TSource, TTarget> autoMapperBuilder;

        public TargetMemberAutoMapperBuilder(string memberName, AutoMapperBuilder<TSource, TTarget> autoMapperBuilder)
        {
            this.autoMapperBuilder = autoMapperBuilder;
            this.memberName = memberName;
        }

        public IAutoMapperBuilder<TSource, TTarget> DefaultValue<TMember>(Func<TMember> valueProvider)
        {

            autoMapperBuilder.targetMemberConfiguration.Add(memberName, new AutoMapperTargetMemberConfiguration(false, (Func<object>)valueProvider);
        }

        public IAutoMapperBuilder<TSource, TTarget> Inject()
        {
            throw new NotImplementedException();
        }
    }
}
