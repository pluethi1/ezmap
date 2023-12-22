using EZMap.Configuration;
using EZMap.Mappers;
using EZMap.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace EZMap.Builders
{
    public class EZMapBuilder : IEZMapBuilder
    {
        private readonly MapperRegistrationCollection registrations = new();
        private readonly MapperSettings mapperSettings = new();
        private bool useCachedResolver;

        public IEZMapBuilder RegisterMapper(Type mapperType, Type sourceType, Type targetType)
        {
            registrations.Add(sourceType, targetType, mapperType);
            return this;
        }

        public IEZMapBuilder UseCachedResolver(bool value = true)
        {
            ConfigureAutoMapper<DateTime, DateOnly>(builder =>
            {
                builder.Source(x => x.Day).MapTo(x => x.Day)
                       .Target(x => x.Year).DefaultValue(() => 1000)
                       .Target(x => x.Month).Inject()
            });
            useCachedResolver = value;
            return this;

        }

        public IEZMapBuilder ConfigureAutoMapper<TSource, TTarget>(Action<IAutoMapperBuilder<TSource, TTarget>> configure)
        {
            return this;
        }

        public void RegisterServices(IServiceCollection serviceCollection)
        {
            // Register registrations
            serviceCollection.AddSingleton(registrations);

            //Register settings
            serviceCollection.AddSingleton(mapperSettings);

            //Register resolver
            Type resolverType;
            if (useCachedResolver)
            {
                resolverType = typeof(CachedMapperResolver);
            }
            else
            {
                resolverType = typeof(DefaultMapperResolver);
            }
            serviceCollection.AddSingleton(typeof(IMapperResolver), resolverType);
            
            // Register Mappers
            serviceCollection.AddTransient<IMapper, AutoMapper>();
            serviceCollection.AddTransient<IAsyncMapper, AsyncAutoMapper>();
            serviceCollection.AddTransient(typeof(ITypeMapper<,>), typeof(ResolvingTypeMapper<,>));
            serviceCollection.AddTransient(typeof(IAsyncTypeMapper<,>), typeof(ResolvingAsyncTypeMapper<,>));
        }
    }
}
