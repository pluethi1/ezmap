using EZMap.Mappers;
using EZMap.Mappers.Wrappers;
using EZMap.Utilities;

namespace EZMap.Registration.Activation
{
    internal static class MapperActivator
    {
        internal static object Create(MapperActivationContext context)
        {
            var requestedType = context.RequestedType;

            if (requestedType.IsGenericType && !requestedType.IsGenericTypeDefinition)
            {
                requestedType = requestedType.GetGenericTypeDefinition();
            }

            if (requestedType == typeof(ITypeMapper<,>) || requestedType == typeof(ITypeMapper))
            {
                return CreateGenericTypeMapper(context);
            }

            if (requestedType == typeof(IAsyncTypeMapper<,>) || requestedType == typeof(IAsyncTypeMapper))
            {
                return CreateGenericAsyncTypeMapper(context);
            }

            throw new InvalidOperationException("Failed to create mapper"); //ToDo better logging/debugging
        }

        private static ITypeMapper CreateGenericTypeMapper(MapperActivationContext context)
        {
            if (context.RegisteredType == null)
            {
                var mapper = CreateAutoMapper(context);
                return CreateGenericInstance<ITypeMapper>(typeof(DeferredTypeMapper<,>), context, mapper);
            }

            if (context.RegisteredType.IsAssignableToAny(CreateGenericTypeMapperType(context, async: false)))
            {
                return (ITypeMapper)MapperUtilities.CreateObject(context.RegisteredType, context.ServiceProvider);
            }

            if (context.RegisteredType.IsAssignableTo(CreateGenericTypeMapperType(context, async: true)))
            {
                var asyncTypeMapper = CreateGenericAsyncTypeMapper(context);
                return CreateGenericInstance<ITypeMapper>(typeof(AsyncTypeMapperWrapper<,>), context, asyncTypeMapper);
            }

            throw new InvalidOperationException("Failed to create mapper"); //ToDo better logging/debugging
        }

        private static IAsyncTypeMapper CreateGenericAsyncTypeMapper(MapperActivationContext context)
        {
            if (context.RegisteredType == null)
            {
                var autoMapper = CreateAutoMapper(context);
                return CreateGenericInstance<IAsyncTypeMapper>(typeof(DeferredAsyncTypeMapper<,>), context, autoMapper);
            }

            if (context.RegisteredType.IsAssignableTo(CreateGenericTypeMapperType(context, async: true)))
            {
                return (IAsyncTypeMapper)MapperUtilities.CreateObject(context.RegisteredType, context.ServiceProvider);
            }

            if (context.RegisteredType.IsAssignableTo(CreateGenericTypeMapperType(context, async: false)))
            {
                var typeMapper = CreateGenericTypeMapper(context);
                return CreateGenericInstance<IAsyncTypeMapper>(typeof(SyncTypeMapperWrapper<,>), context, typeMapper);
            }

            throw new InvalidOperationException("Failed to create mapper"); //ToDo better logging/debugging
        }

        private static IMapper CreateAutoMapper(MapperActivationContext context)
        {
            return new AutoMapper(context.MapperSettings, context.MapperResolver, context.ServiceProvider);
        }

        private static Type CreateGenericTypeMapperType(MapperActivationContext context, bool async)
        {
            if (async)
            {
                return typeof(IAsyncTypeMapper<,>).MakeGenericType(context.SourceType, context.TargetType);
            }

            return typeof(ITypeMapper<,>).MakeGenericType(context.SourceType, context.TargetType);
        }

        private static T CreateGenericInstance<T>(Type type, MapperActivationContext context, params object?[]? args)
        {
            //return type.MakeGenericType(context.SourceType, context.TargetType);
            return (T)Activator.CreateInstance(type.MakeGenericType(context.SourceType, context.TargetType), args)!;
        }
    }
}
