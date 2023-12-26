using EZMap.Configuration;
using EZMap.Mappers;
using EZMap.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace EZMap.Builders
{
    public class EZMapBuilder : IEZMapBuilder
    {
        private readonly MapperSettings mapperSettings = MapperSettings.Default;
        private bool useCachedResolver;

        public IEZMapBuilder RegisterMapper(Type mapperType, Type sourceType, Type targetType)
        {
            mapperSettings.Registrations.Add(sourceType, targetType, mapperType);
            return this;
        }

        public IEZMapBuilder UseCachedResolver(bool value = true)
        {
            useCachedResolver = value;
            return this;

        }

        public IEZMapBuilder ConfigureAutoMapper<TSource, TTarget>(Action<IAutoMapperBuilder<TSource, TTarget>> configure)
        {
            var builder = new AutoMapperBuilder<TSource, TTarget>();
            configure(builder);
            var autoMapperConfiguration = builder.Build();
            mapperSettings.Registrations.Add(typeof(TSource), typeof(TTarget), autoMapperConfiguration);
            return this;
        }

        public void RegisterServices(IServiceCollection serviceCollection)
        {
            //Register settings
            serviceCollection.AddSingleton(mapperSettings);

            //Register resolver
            Func<IServiceProvider, object> resolverFactory;
            if (useCachedResolver)
            {
                resolverFactory = _ => new CachedMapperResolver();
            }
            else
            {
                resolverFactory = provider => new DefaultMapperResolver(provider.GetRequiredService<MapperSettings>(),
                                                                        provider);
            }
            serviceCollection.AddSingleton(typeof(IMapperResolver), resolverFactory);
            
            // Register Mappers
            serviceCollection.AddTransient<IMapper, AutoMapper>();
            serviceCollection.AddTransient(typeof(ITypeMapper<,>), typeof(ResolvingTypeMapper<,>));
            serviceCollection.AddTransient(typeof(IAsyncTypeMapper<,>), typeof(ResolvingAsyncTypeMapper<,>));
        }
    }
}