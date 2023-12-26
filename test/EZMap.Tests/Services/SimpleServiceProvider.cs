using Microsoft.Extensions.DependencyInjection;

namespace EZMap.Tests.Services
{
    public class SimpleServiceProvider : IServiceProvider
    {
        private readonly IServiceCollection services;

        public SimpleServiceProvider(IServiceCollection services)
        {
            this.services = services;
        }

        public object? GetService(Type serviceType)
        {
            if (serviceType == typeof(IServiceProvider)) 
            {
                return this;
            }

            var descriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == serviceType);
            if (descriptor == null)
            {
                return null;
            }
            if (descriptor.ImplementationFactory != null)
            {
                return descriptor.ImplementationFactory(this);
            }
            throw new NotSupportedException();
        }
    }
}
