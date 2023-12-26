using EZMap.Builders;
using EZMap.Tests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EZMap.Tests
{
    public static class TestUtilities
    {
        public static IServiceCollection CreateDefaultServices(int age = 20, string name = "Peter", Action<IEZMapBuilder>? buildAction = null)
        {
            var services = new ServiceCollection();
            var builder = new EZMapBuilder();
            buildAction?.Invoke(builder);
            builder.RegisterServices(services);
            services.AddTransient<IAgeProvider>(_ => new DefaultAgeProvider(age));
            services.AddTransient<INameProvider>(_ => new DefaultNameProvider(name));
            return services;
        }

        public static IServiceProvider CreateDefaultServiceProvider()
        {
            return CreateDefaultServiceProvider(CreateDefaultServices());
        }

        public static IServiceProvider CreateDefaultServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }
    }
}
