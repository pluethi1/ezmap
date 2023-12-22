using EZMap.Registration;

namespace EZMap.Mappers
{
    // A limitation of Microsoft.Extensions.DependencyInjection disallows factories for open generics
    // so we have a wrapper type that resolves the mappers instead
    internal class ResolvingAsyncTypeMapper<TSource, TTarget> : IAsyncTypeMapper<TSource, TTarget>
        where TSource : notnull
        where TTarget : notnull
    {
        private readonly IMapperResolver mapperResolver;
        private IAsyncTypeMapper<TSource, TTarget>? resolvedAsyncTypeMapper;

        public ResolvingAsyncTypeMapper(IMapperResolver mapperResolver)
        {
            this.mapperResolver = mapperResolver;
        }

        public Type SourceType => EnsureResolved().SourceType;
        public Type TargetType => EnsureResolved().TargetType;

        public Task<TTarget> MapAsync(TSource source, CancellationToken cancellationToken = default)
        {
            return EnsureResolved().MapAsync(source, cancellationToken);
        }

        public Task<object> MapAsync(object source, CancellationToken cancellationToken = default)
        {
            return EnsureResolved().MapAsync(source, cancellationToken);
        }

        private IAsyncTypeMapper<TSource, TTarget> EnsureResolved()
        {
            return resolvedAsyncTypeMapper ??= mapperResolver.ResolveAsyncTypeMapper<TSource, TTarget>();
        }
    }
}