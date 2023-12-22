using EZMap.Registration;

namespace EZMap.Mappers
{
    // A limitation of Microsoft.Extensions.DependencyInjection disallows factories for open generics
    // so we have a wrapper type that resolves the mappers instead
    internal class ResolvingTypeMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget>
        where TSource : notnull
        where TTarget : notnull
    {
        private readonly IMapperResolver mapperResolver;
        private ITypeMapper<TSource, TTarget>? resolvedTypeMapper;

        public ResolvingTypeMapper(IMapperResolver mapperResolver) 
        { 
            this.mapperResolver = mapperResolver;
        }

        public Type SourceType => EnsureResolved().SourceType;
        public Type TargetType => EnsureResolved().TargetType;

        public TTarget Map(TSource source)
        {
            return EnsureResolved().Map(source);
        }

        public object Map(object source)
        {
            return EnsureResolved().Map(source);
        }

        private ITypeMapper<TSource, TTarget> EnsureResolved()
        {
            return resolvedTypeMapper ??= mapperResolver.ResolveTypeMapper<TSource, TTarget>();
        }

    }
}
