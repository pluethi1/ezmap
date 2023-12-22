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

            if (requestedType == typeof(IMapper))
            {
                return CreateMapper(context);
            }

            if (requestedType == typeof(IAsyncMapper))
            {
                return CreateAsyncMapper(context);
            }

            if (requestedType == typeof(ITypeMapper))
            {
                return CreateTypeMapper(context);
            }

            if (requestedType == CreateGenericTypeMapperType(context, false))
            {
                return CreateGenericTypeMapper(context);
            }

            if (requestedType == typeof(IAsyncTypeMapper))
            {
                return CreateAsyncTypeMapper(context);
            }

            if (requestedType == CreateGenericTypeMapperType(context, true))
            {
                return CreateGenericAsyncTypeMapper(context);
            }

            throw new InvalidOperationException("Failed to create mapper"); //ToDo better logging/debugging
        }

        private static IMapper CreateMapper(MapperActivationContext context)
        {
            if (context.RegisteredType == null)
            {
                return CreateAutoMapper(context);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IMapper)))
            {
                return (IMapper)MapperUtilities.CreateObject(context.RegisteredType, context.ServiceProvider);
            }
            
            if (context.RegisteredType.IsAssignableTo(typeof(ITypeMapper)))
            {
                var typeMapper = CreateTypeMapper(context);
                return new DeferredMapper(typeMapper);
            }

            if (context.RegisteredType.IsAssignableToAny(typeof(IAsyncMapper), typeof(IAsyncTypeMapper)))
            {
                var asyncMapper = CreateAsyncMapper(context);
                return new AsyncMapperWrapper(asyncMapper);
            }

            throw new InvalidOperationException("Failed to create mapper"); //ToDo better logging/debugging

        }

        private static IAsyncMapper CreateAsyncMapper(MapperActivationContext context)
        {
            if (context.RegisteredType == null)
            {
                return CreateAsyncAutoMapper(context);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IAsyncMapper)))
            {
                return (IAsyncMapper)MapperUtilities.CreateObject(context.RegisteredType, context.ServiceProvider);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IAsyncTypeMapper)))
            {
                var asyncTypeMapper = CreateAsyncTypeMapper(context);
                return new DeferredAsyncMapper(asyncTypeMapper);
            }

            if (context.RegisteredType.IsAssignableToAny(typeof(IMapper), typeof(ITypeMapper)))
            {
                var mapper = CreateMapper(context);
                return new SyncMapperWrapper(mapper);
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

            if (context.RegisteredType.IsAssignableTo(CreateGenericTypeMapperType(context, false)))
            {
                return (ITypeMapper)MapperUtilities.CreateObject(context.RegisteredType, context.ServiceProvider);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IMapper)))
            {
                var mapper = CreateMapper(context);
                return CreateGenericInstance<ITypeMapper>(typeof(DeferredTypeMapper<,>), context, mapper);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(ITypeMapper)))
            {
                var typeMapper = CreateTypeMapper(context);
                return CreateGenericInstance<ITypeMapper>(typeof(DeferredGenericTypeMapper<,>), context, typeMapper);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IAsyncMapper)))
            {
                var asyncMapper = CreateAsyncMapper(context);
                var mapper = new AsyncMapperWrapper(asyncMapper);
                return CreateGenericInstance<ITypeMapper>(typeof(DeferredTypeMapper<,>), context, mapper);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IAsyncTypeMapper)))
            {
                var asyncTypeMapper = CreateGenericAsyncTypeMapper(context);
                return CreateGenericInstance<ITypeMapper>(typeof(AsyncTypeMapperWrapper<,>), context, asyncTypeMapper);
            }

            throw new InvalidOperationException("Failed to create mapper"); //ToDo better logging/debugging
        }

        private static ITypeMapper CreateTypeMapper(MapperActivationContext context)
        {
            if (context.RegisteredType == null)
            {
                var autoMapper = CreateAutoMapper(context);
                return new DeferredTypeMapper(context.SourceType, context.TargetType, autoMapper);
            }

            if (context.RegisteredType.IsAssignableTo(CreateGenericTypeMapperType(context, false)))
            {
                return CreateGenericTypeMapper(context);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(ITypeMapper)))
            {
                return (ITypeMapper)MapperUtilities.CreateObject(context.RegisteredType, context.ServiceProvider);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IMapper)))
            {
                var mapper = CreateMapper(context);
                return new DeferredTypeMapper(context.SourceType, context.TargetType, mapper);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IAsyncMapper)))
            {
                var asyncMapper = CreateAsyncMapper(context);
                var mapper = new AsyncMapperWrapper(asyncMapper);
                return new DeferredTypeMapper(context.SourceType, context.TargetType, mapper);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IAsyncTypeMapper)))
            {
                var asyncTypeMapper = CreateAsyncTypeMapper(context);
                return new AsyncTypeMapperWrapper(asyncTypeMapper);
            }

            throw new InvalidOperationException("Failed to create mapper"); //ToDo better logging/debugging
        }

        private static IAsyncTypeMapper CreateGenericAsyncTypeMapper(MapperActivationContext context)
        {
            if (context.RegisteredType == null)
            {
                var asyncAutoMapper = CreateAsyncAutoMapper(context);
                return CreateGenericInstance<IAsyncTypeMapper>(typeof(DeferredAsyncTypeMapper<,>), context, asyncAutoMapper);
            }

            if (context.RegisteredType.IsAssignableTo(CreateGenericTypeMapperType(context, true)))
            {
                return (IAsyncTypeMapper)MapperUtilities.CreateObject(context.RegisteredType, context.ServiceProvider);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IAsyncMapper)))
            {
                var asyncMapper = CreateAsyncMapper(context);
                return CreateGenericInstance<IAsyncTypeMapper>(typeof(DeferredAsyncTypeMapper<,>), context, asyncMapper);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IAsyncTypeMapper)))
            {
                var asyncTypeMapper = CreateAsyncTypeMapper(context);
                return CreateGenericInstance<IAsyncTypeMapper>(typeof(DeferredGenericAsyncTypeMapper<,>), context, asyncTypeMapper);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(ITypeMapper)))
            {
                var typeMapper = CreateGenericTypeMapper(context);
                return CreateGenericInstance<IAsyncTypeMapper>(typeof(SyncTypeMapperWrapper<,>), context, typeMapper);
            }

            throw new InvalidOperationException("Failed to create mapper"); //ToDo better logging/debugging
        }

        private static IAsyncTypeMapper CreateAsyncTypeMapper(MapperActivationContext context)
        {
            if (context.RegisteredType == null)
            {
                var asyncAutoMapper = CreateAsyncAutoMapper(context);
                return new DeferredAsyncTypeMapper(context.SourceType, context.TargetType, asyncAutoMapper);
            }

            if (context.RegisteredType.IsAssignableTo(CreateGenericTypeMapperType(context, true)))
            {
                return CreateGenericAsyncTypeMapper(context);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IAsyncTypeMapper)))
            {
                return (IAsyncTypeMapper)MapperUtilities.CreateObject(context.RegisteredType, context.ServiceProvider);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IAsyncMapper)))
            {
                var asyncMapper = CreateAsyncMapper(context);
                return new DeferredAsyncTypeMapper(context.SourceType, context.TargetType, asyncMapper);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(IMapper)))
            {
                var mapper = CreateMapper(context);
                var asyncMapper = new SyncMapperWrapper(mapper);
                return new DeferredAsyncTypeMapper(context.SourceType, context.TargetType, asyncMapper);
            }

            if (context.RegisteredType.IsAssignableTo(typeof(ITypeMapper)))
            {
                var typeMapper = CreateTypeMapper(context);
                return new SyncTypeMapperWrapper(typeMapper);
            }

            throw new InvalidOperationException("Failed to create mapper"); //ToDo better logging/debugging
        }

        private static IMapper CreateAutoMapper(MapperActivationContext context)
        {
            return new AutoMapper(context.MapperSettings, context.MapperResolver, context.ServiceProvider);
        }

        private static IAsyncMapper CreateAsyncAutoMapper(MapperActivationContext context)
        {
            return new AsyncAutoMapper(context.MapperSettings, context.MapperResolver, context.ServiceProvider);
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
